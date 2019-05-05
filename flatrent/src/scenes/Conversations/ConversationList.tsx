import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader";
import { toast } from "react-toastify";
import Styles from "./ConversationList.module.css";

import InfiniteScroll from "react-infinite-scroller";
import PerfectScrollbar from "react-perfect-scrollbar";
import "react-perfect-scrollbar/dist/css/styles.css";
import FlexColumn from "../../components/FlexColumn";
import { avatarUrl } from "../../services/ApiUtilities";
import ConversationService from "../../services/ConversationService";
import { IConversationDetails } from "../../services/interfaces/ConversationInterfaces";
import UserService from "../../services/UserService";
import { joined } from "../../utilities/Utilities";

interface IConversationListProps {
    className?: string;
    onSelected: (conversation: IConversationDetails) => void;
}

interface IConversationListState {
    hasMore: boolean;
    conversations: IConversationDetails[];
}

class ConversationList extends Component<IConversationListProps, IConversationListState> {
    constructor(props: IConversationListProps) {
        super(props);
        this.state = { hasMore: true, conversations: [] };
    }

    public render() {
        const { conversations } = this.state;

        return (
            <FlexColumn className={joined(Styles.content, this.props.className)}>
                <span className={Styles.title}>Susirašinėjimai</span>
                <PerfectScrollbar>
                    <InfiniteScroll
                        className={Styles.scroller}
                        pageStart={0}
                        initialLoad={true}
                        loadMore={this.fetchConversations}
                        hasMore={this.state.hasMore}
                        loader={<InvoiceLoader key={0} />}
                        useWindow={false}
                        isReverse={true}
                    >
                        {this.getItems(conversations)}
                    </InfiniteScroll>
                </PerfectScrollbar>
            </FlexColumn>
        );
    }

    private getItems = (conversations?: IConversationDetails[]): ReactNode[] => {
        if (conversations === undefined) {
            return Array(3)
                .fill(0)
                .map((_, idx) => <InvoiceLoader key={idx} />);
        }

        return conversations.map((x) => {
            const user = UserService.userId() === x.author.id ? x.recipient : x.author;
            return (
                <FlexColumn onClick={this.onConversationClickedFactory(x)} key={x.id} className={Styles.item}>
                    <span className={Styles.itemTitle}>
                        {user.firstName} {user.lastName}
                    </span>
                    <div className={Styles.avatarWrapper}>
                        <img alt="User avatar" className={Styles.avatar} src={avatarUrl(user.id)} />
                    </div>
                </FlexColumn>
            );
        });
    };

    private onConversationClickedFactory = (conversation: IConversationDetails) => (
        _: React.MouseEvent<HTMLDivElement>
    ) => {
        this.props.onSelected(conversation);
    };

    private fetchConversations = async (_: number) => {
        try {
            const { errors, data } = await ConversationService.getConversations(this.state.conversations.length);
            if (errors !== undefined) {
                const error = Object.keys(errors).map((key) => errors![key].join("\n"));
                error.forEach((err) => toast.error(err));
            } else if (data !== undefined) {
                console.log(data);
                this.setState((state) => {
                    const uniqueConversations = state.conversations;
                    data.forEach((conv) => {
                        if (uniqueConversations.findIndex((uc) => uc.id === conv.id) === -1) {
                            uniqueConversations.push(conv);
                        }
                    });
                    return {
                        conversations: uniqueConversations,
                        hasMore: data.length === 16,
                    };
                });
            }
        } catch (error) {
            console.log(error);
            toast.error("Įvyko nežinoma klaida.");
        }
    };
}

export const InvoiceLoader = () => (
    <div className={Styles.flatBox}>
        <div className={Styles.loader}>
            <ContentLoader height={140} width={180} speed={2} primaryColor="#f3f3f3" secondaryColor="#ecebeb">
                <rect x="30" y="10" rx="3" ry="3" width="120" height="8" />
                <circle cx="90" cy="65" r="40" />
            </ContentLoader>
        </div>
    </div>
);

export default ConversationList;
