import React from "react";
import AttachmentService from "../../services/AttachmentService";
import { IAttachment } from "../../services/interfaces/Common";
import { joined } from "../../utilities/Utilities";
import FlexColumn from "../FlexColumn";
import Styles from "./CompactAttachmentPreview.module.css";

export interface ICompactAttachmentPreview {
  className?: string;
  attachments: IAttachment[];
}

function CompactAttachmentPreview({ className, attachments }: ICompactAttachmentPreview) {
  const downloadLinkFactory = (id: string, name: string) => () => {
    if (id !== "") {
      AttachmentService.downloadAttachment(id, name);
    }
  };
  if (attachments.length === 0) {
    attachments.push({ id: "", name: "Nėra", mime: "" });
  }

  const thumbs = attachments.map((file) => (
    <div onClick={downloadLinkFactory(file.id, file.name)} className={Styles.attachment} key={file.name}>
      <span className={Styles.textThumb}>{file.name}</span>
    </div>
  ));

  return <FlexColumn className={joined(Styles.attachmentContainer, className)}>{thumbs}</FlexColumn>;
}

export default CompactAttachmentPreview;
