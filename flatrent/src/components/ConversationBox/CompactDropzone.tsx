import React, { useCallback, useState, useEffect } from "react";
import { useDropzone } from "react-dropzone";
import Styles from "./CompactDropzone.module.css";
import { joined } from "../../utilities/Utilities";
import FlexColumn from "../FlexColumn";

export interface ICompactDropzone {
  onDrop: (acceptedFiles: File[]) => void;
  minSize?: number;
  maxSize?: number;
  maxFiles?: number;
  accept?: string | string[];
  text: string;
  className?: string;
  files: File[];
}

function CompactDropzone({ files, text, className, onDrop, minSize, maxSize, maxFiles = 8, accept }: ICompactDropzone) {
  const { getRootProps, getInputProps } = useDropzone({
    accept,
    maxSize,
    minSize,
    multiple: true,
    onDrop: (acceptedFiles) => {
      if (files.length >= maxFiles) {
        return;
      }
      const newFiles = acceptedFiles.filter(
        (af) => files.findIndex((f) => f.name === af.name && f.size === af.size) === -1
      );
      if (newFiles.length === 0) {
        return;
      }
      if (newFiles.length + files.length > maxFiles) {
        const oversizedBy = files.length + newFiles.length - maxFiles;

        for (let idx = 0; idx < oversizedBy; idx++) {
          newFiles.pop();
        }
      }

      const newFilesArray = [...files, ...newFiles];
      onDrop(newFilesArray);
    },
  });

  const removeItem = (name: string) => () => {
    const leftFiles = files.filter((f) => f.name !== name);
    onDrop(leftFiles);
  };

  const thumbs = files.map((file) => (
    <span onClick={removeItem(file.name)} className={Styles.thumb} key={file.name}>
      {file.name}
    </span>
  ));

  return (
    <section className={joined(Styles.dropzoneContainer, className)}>
      <div {...getRootProps({ className: Styles.dropzone })}>
        <input {...getInputProps()} />
        <p>{text}</p>
      </div>
      <FlexColumn className={Styles.thumbsContainer}>
        {thumbs}
      </FlexColumn>
    </section>
  );
}

export default CompactDropzone;
