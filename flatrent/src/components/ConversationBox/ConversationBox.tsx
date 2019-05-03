import Moment from "moment";
import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader";
import InfiniteScroll from "react-infinite-scroller";
import PerfectScrollbar from "react-perfect-scrollbar";
import "react-perfect-scrollbar/dist/css/styles.css";
import { toast } from "react-toastify";
import ConversationService from "../../services/ConversationService";
import { IAttachment } from "../../services/interfaces/Common";
import { IConversationDetails, IMessageDetails } from "../../services/interfaces/ConversationInterfaces";
import UserService from "../../services/UserService";
import { joined } from "../../utilities/Utilities";
import CompactAttachmentPreview from "../AttachmentPreview/CompactAttachmentPreview";
import Button from "../Button";
import { CompactDropzone } from "../Dropzones";
import FlexColumn from "../FlexColumn";
import FlexRow from "../FlexRow";
import { InputAreaForm } from "../InputForm";
import Styles from "./ConversationBox.module.css";

interface IConversationBoxProps {
  className?: string;
  conversation: IConversationDetails;
}

interface IConversationBoxState {
  writtenMessage: string;
  messages: IMessageDetails[];
  hasMore: boolean;
  scrollRef: HTMLDivElement | null;
  firstLoadFinished: boolean;
  files: File[];
}

const messageCompare = (a: IMessageDetails, b: IMessageDetails) => {
  if (a.createdDate < b.createdDate) {
    return 1;
  }
  if (a.createdDate > b.createdDate) {
    return -1;
  }
  return 0;
};

class ConversationBox extends Component<IConversationBoxProps, IConversationBoxState> {
  private scrollRef: HTMLDivElement | null = null;
  private newMessage: boolean = false;
  private shouldCheckForMessages: boolean = false;

  constructor(props: IConversationBoxProps) {
    super(props);
    this.state = {
      writtenMessage: "",
      hasMore: true,
      messages: [],
      scrollRef: null,
      firstLoadFinished: false,
      files: [],
    };
  }

  public componentDidMount() {
    this.shouldCheckForMessages = true;
    this.loadNewMessages();
  }

  public componentWillUnmount() {
    this.shouldCheckForMessages = false;
  }

  public render() {
    return (
      <FlexColumn className={joined(Styles.conversationBox, this.props.className)}>
        <span className={Styles.title}>Susirašinėjimas</span>
        <span className={Styles.subject}>{this.props.conversation.subject}</span>
        <FlexColumn className={Styles.messages}>
          <PerfectScrollbar>
            <InfiniteScroll
              className={Styles.scroller}
              pageStart={0}
              initialLoad={true}
              loadMore={this.loadMessages}
              hasMore={this.state.hasMore}
              loader={<MessageLoader key={0} />}
              useWindow={false}
              isReverse={true}
            >
              {this.getMessages()}
            </InfiniteScroll>
          </PerfectScrollbar>
          <FlexRow>
            <InputAreaForm
              name="message"
              title="Žinutė"
              maxChars={5000}
              value={this.state.writtenMessage}
              setValue={this.updateMessageContent}
              className={Styles.messageInput}
            />
            <CompactDropzone
              className={Styles.dropzone}
              maxFiles={3}
              maxSize={5000000}
              files={this.state.files}
              onDrop={this.updateMessageFiles}
              text="Pridėti failus (3 maks.)"
            />
          </FlexRow>
          <Button className={Styles.send} onClick={this.sendMessage}>
            Siųsti žinutę
          </Button>
        </FlexColumn>
      </FlexColumn>
    );
  }

  public componentDidUpdate() {
    if (this.scrollRef !== null && this.newMessage) {
      this.scrollRef.scrollIntoView();
      this.newMessage = false;
    }
    if (this.scrollRef !== null && this.scrollRef!.parentElement!.parentElement!.parentElement !== null) {
      const parel = this.scrollRef!.parentElement!.parentElement!.parentElement;
      if (parel.scrollHeight - parel.scrollTop - parel.offsetHeight <= parel.clientHeight) {
        this.scrollRef.scrollIntoView();
      }
    }
  }

  private updateMessageContent = (_: string, writtenMessage: string) => this.setState({ writtenMessage });
  private updateMessageFiles = (files: File[]) => this.setState({ files });

  private getMessages = (): ReactNode => {
    const userId = UserService.userId();
    const { messages } = this.state;
    const mapped = messages
      .sort(messageCompare)
      .map((message, idx) => {
        const userMessageStyle = userId === message.authorId ? Styles.authorMessage : Styles.recipientMessage;
        const split = message.createdDate.split("T");
        const label = `${split[0]} ${split[1].split(".")[0]}`;
        const refOrNot =
          idx === 0
            ? (ref: HTMLDivElement | null) => {
                this.scrollRef = ref;
              }
            : () => {};
        return (
          <div ref={refOrNot} key={message.id} className={userMessageStyle}>
            <span className={Styles.messageText}>{message.content}</span>
            {this.getAttachmentBox(message)}
            <span className={Styles.messageLabel}>{label}</span>
          </div>
        );
      })
      .reverse();
    return <div className={Styles.messageContainer}>{mapped}</div>;
  };

  private getAttachmentBox = (message: IMessageDetails): ReactNode => {
    if (message.attachments === undefined || message.attachments.length === 0) {
      return <></>;
    }
    return <CompactAttachmentPreview attachments={message.attachments} className={Styles.attachments} />;
  };

  private loadMessages = async () => {
    const response = await ConversationService.getMessages(this.props.conversation.id, this.state.messages.length);
    if (response.errors !== undefined) {
      const errors = Object.keys(response.errors).map((key) => response.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
      return;
    }

    if (response.data !== undefined) {
      this.setState((state) => ({
        hasMore: response.data!.length >= 32,
        messages: [...state.messages, ...response.data!],
      }));
    }
  };

  private loadNewMessages = async () => {
    if (!this.shouldCheckForMessages) {
      return;
    }
    const sorted = this.state.messages.sort(messageCompare);
    if (this.state.messages.length !== 0) {
      const response = await ConversationService.getNewMessages(this.props.conversation.id, sorted[0].id);
      if (response.errors !== undefined) {
        const errors = Object.keys(response.errors).map((key) => response.errors![key].join("\n"));
        errors.forEach((error) => toast.error(error));
        return;
      }

      if (response.data !== undefined && response.data.length > 0) {
        this.setState((state) => ({
          messages: [...response.data!, ...state.messages],
        }));
      }
    }
    setTimeout(() => this.loadNewMessages(), 5000);
  };

  private sendMessage = async () => {
    const { writtenMessage, files } = this.state;
    const response = await ConversationService.createMessage(this.props.conversation.id, writtenMessage, files);
    if (response.errors !== undefined) {
      const errors = Object.keys(response.errors).map((key) => response.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
      return;
    }

    const remapped: IAttachment[] = Object.keys(response.data!.attachments).map((k) => ({
      id: k,
      mime: "",
      name: response.data!.attachments[k],
    }));

    const message: IMessageDetails = {
      attachments: remapped,
      authorId: UserService.userId(),
      content: writtenMessage,
      createdDate: Moment.utc().toISOString(),
      id: response.data!.id,
    };

    this.newMessage = true;
    this.setState((state) => ({
      files: [],
      messages: [message, ...state.messages],
      writtenMessage: "",
    }));
  };
}

const MessageLoader = () => (
  <ContentLoader key={0} speed={2} height={92} width={520} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
    <rect x="248" y="4" rx="4" ry="4" width="260" height="38" />
    <rect x="12" y="46" rx="4" ry="4" width="230" height="38" />
  </ContentLoader>
);

export default ConversationBox;
