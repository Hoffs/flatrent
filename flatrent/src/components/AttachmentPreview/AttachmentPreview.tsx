import React from "react";
import AttachmentService from "../../services/AttachmentService";
import { IAttachment } from "../../services/interfaces/Common";
import { joined } from "../../utilities/Utilities";
import FlexRow from "../FlexRow";
import Styles from "./AttachmentPreview.module.css";

export interface IAttachmentPreview {
    className?: string;
    attachments: IAttachment[];
}

function AttachmentPreview({ className, attachments }: IAttachmentPreview) {
    const downloadLinkFactory = (id: string, name: string) => () => AttachmentService.downloadAttachment(id, name);

    const getThumbContent = (file: IAttachment) => {
        return <span className={Styles.textThumb}>{file.name}</span>;
    };

    const thumbs = attachments.map((file) => (
        <div onClick={downloadLinkFactory(file.id, file.name)} className={Styles.attachmentThumb} key={file.name}>
            {getThumbContent(file)}
        </div>
    ));

    return <FlexRow className={joined(Styles.attachmentContainer, className)}>{thumbs}</FlexRow>;
}

export default AttachmentPreview;
