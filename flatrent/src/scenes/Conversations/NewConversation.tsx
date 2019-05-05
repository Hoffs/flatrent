import React from "react";
import { RouteComponentProps } from "react-router-dom";
import ConversationService from "../../services/ConversationService";
import { conversationUrl } from "../../utilities/Utilities";

const NewConversation = (props: RouteComponentProps) => {
    const search = new URLSearchParams(props.location.search);
    console.log("creating", search);
    ConversationService.newConversation(search.get("recipientId") as string)
        .then((response) => {
            if (response.data !== undefined) {
                props.history.push(conversationUrl(response.data.id));
                return;
            }
            props.history.push(conversationUrl(""));
        })
        .catch(() => {
            props.history.push(conversationUrl(""));
        });

    return <></>;
};

export default NewConversation;
