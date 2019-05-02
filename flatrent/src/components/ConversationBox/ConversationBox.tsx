import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader";
import InfiniteScroll from "react-infinite-scroller";
import { toast } from "react-toastify";
import ConversationService from "../../services/ConversationService";
import { IConversationDetails, IMessageDetails } from "../../services/interfaces/ConversationInterfaces";
import UserService from "../../services/UserService";
import { joined } from "../../utilities/Utilities";
import FlexColumn from "../FlexColumn";
import Styles from "./ConversationBox.module.css";
import PerfectScrollbar from "react-perfect-scrollbar";
import "react-perfect-scrollbar/dist/css/styles.css";
import Button from "../Button";
import { InputAreaForm } from "../InputForm";
import AttachmentPreview from "../AttachmentPreview";
import Moment from "moment";

interface IConversationBoxProps {
  className?: string;
  conversation: IConversationDetails;
}

interface IConversationBoxState {
  writtenMessage: string;
  messages: IMessageDetails[];
  hasMore: boolean;
}

class ConversationBox extends Component<IConversationBoxProps, IConversationBoxState> {
  constructor(props: IConversationBoxProps) {
    super(props);
    this.state = { writtenMessage: "", hasMore: true, messages: [] };
    this.loadMessages();
  }

  public render() {
    return (
    <FlexColumn className={joined(Styles.conversationBox, this.props.className)}>
      <span className={Styles.title}>Susirašinėjimas</span>
      <span className={Styles.subject}>{this.props.conversation.subject}</span>
      <InputAreaForm
        name="message"
        title="Žinutė"
        maxChars={5000}
        setValue={this.updateMessageContent}
        className={Styles.messageInput}
      />
      <Button className={Styles.send} onClick={this.sendMessage}>Siųsti žinutę</Button>
      <FlexColumn className={Styles.messages}>
        <PerfectScrollbar>
          <InfiniteScroll
            className={Styles.scroller}
            pageStart={0}
            initialLoad={false}
            loadMore={this.loadMessages}
            hasMore={this.state.hasMore}
            loader={<MessageLoader />}
            useWindow={false}
          >
              {this.getMessages()}
          </InfiniteScroll>
          {/* <SmartImg className={Styles.authorAvatar} src={avatarUrl(this.props.conversation.author.id)} />
          <SmartImg className={Styles.recipientAvatar} src={avatarUrl(this.props.conversation.recipient.id)} /> */}
        </PerfectScrollbar>
      </FlexColumn>
    </FlexColumn>);
  }

  private updateMessageContent = (_: string, writtenMessage: string) => this.setState({ writtenMessage });

  private getMessages = (): ReactNode => {
    const userId = UserService.userId();
    const mapped = this.state.messages.map((message) => {
      const userMessageStyle = userId === message.authorId ? Styles.author : Styles.recipient;
      const split = message.createdDate.split("T");
      const label = `${split[0]} ${split[1].split(".")[0]}`;
      return (
      <div aria-label={message.createdDate}  key={message.id} className={userMessageStyle}>
        <span className={Styles.messageText}>{message.content}</span>
        {this.getAttachmentBox(message)}
        <span className={Styles.messageLabel}>{label}</span>
      </div>);
    });
    return <div className={Styles.messageContainer}>{mapped}</div>;
  }

  private getAttachmentBox = (message: IMessageDetails): ReactNode => {
    if (message.attachments === undefined || message.attachments.length === 0) { return (<></>); }
    return <AttachmentPreview attachments={message.attachments} className={Styles.attachments} />;
  }

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
  }

  private sendMessage = async () => {
    const toSend = this.state.writtenMessage;
    const response = await ConversationService.createMessage(this.props.conversation.id, toSend);
    if (response.errors !== undefined) {
      const errors = Object.keys(response.errors).map((key) => response.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
      return;
    }

    const message: IMessageDetails = {
      attachments: [],
      authorId: UserService.userId(),
      content: toSend,
      createdDate: Moment.utc().toISOString(),
      id: response.data!.id,
    };

    this.setState((state) => ({
      messages: [
        message,
        ...state.messages,
      ],
    }));
  }
}

const MessageLoader = () => (
  <ContentLoader speed={2} height={92} width={520} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
    <rect x="248" y="4" rx="4" ry="4" width="260" height="38" />
    <rect x="12" y="46" rx="4" ry="4" width="230" height="38" />
  </ContentLoader>
)

export default ConversationBox;
