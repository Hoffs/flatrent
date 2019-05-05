import React, { Component } from "react";
import { RouteComponentProps, Switch } from "react-router-dom";
import ConversationBox from "../../components/ConversationBox";
import FlexRow from "../../components/FlexRow";
import RoleRoute from "../../components/RoleRoute";
import { Authentication } from "../../Routes";
import ConversationService from "../../services/ConversationService";
import { IConversationDetails } from "../../services/interfaces/ConversationInterfaces";
import { conversationUrl } from "../../utilities/Utilities";
import ConversationList from "./ConversationList";
import Styles from "./Conversations.module.css";
import NewConversation from "./NewConversation";

interface IConversationsRouteProps {
    id: string;
}

interface IConversationsState {
    conversation?: IConversationDetails;
}

class Conversations extends Component<RouteComponentProps<IConversationsRouteProps>, IConversationsState> {
    constructor(props: RouteComponentProps<IConversationsRouteProps>) {
        super(props);
        this.state = {};
    }

    public render() {
        const box = this.state.conversation !== undefined
            ? <ConversationBox className={Styles.conversation} conversation={this.state.conversation} />
            : <></>;

        return (
            <FlexRow className={Styles.content}>
                <ConversationList onSelected={this.onConversationSelected} />
                <div className={Styles.chatPlaceholder} >
                    {box}
                    <Switch>
                        <RoleRoute
                            authenticated={Authentication.Authenticated}
                            path={conversationUrl("new")}
                            redirect="/"
                            component={this.getRouteComponent}
                        />
                        <RoleRoute
                            authenticated={Authentication.Authenticated}
                            path={conversationUrl(":id")}
                            redirect="/"
                            component={this.updateBoxComponent}
                        />
                    </Switch>
                </div>
            </FlexRow>
        );
    }

    private getRouteComponent = (props: RouteComponentProps) => <NewConversation {...props} />;
    private updateBoxComponent = (props: RouteComponentProps<{ id: string }>) => {
        if (this.state.conversation === undefined || this.state.conversation.id !== props.match.params.id) {
            ConversationService.getConversation(props.match.params.id).then((result) => {
                if (result.data !== undefined) {
                    this.setState({ conversation: result.data });
                } else {
                    props.history.push(conversationUrl(""));
                }
            }).catch(() => props.history.push(conversationUrl("")));
        }

        return <></>;
    }

    private onConversationSelected = (conversation: IConversationDetails) => this.props.history.push(conversationUrl(conversation.id));

}

export default Conversations;
