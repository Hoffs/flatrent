import React, {useCallback, useState, useEffect} from "react";
import {useDropzone} from "react-dropzone";
import Styles from "./FlexDropzone.module.css";
import { joined } from "../../utilities/Utilities";

export interface IFlexDropzone {
  onDrop: (acceptedFiles: IPreviewFile[]) => void;
  minSize?: number;
  maxSize?: number;
  maxFiles?: number;
  accept?: string | string[];
  text: string;
  className?: string;
}

export interface IPreviewFile extends File {
  preview: string;
}

function FlexDropzone({text, className, onDrop, minSize, maxSize, maxFiles = 8, accept}: IFlexDropzone) {
  const [files, setFiles] = useState<IPreviewFile[]>([]);

  const {getRootProps, getInputProps} = useDropzone({
    accept,
    maxSize,
    minSize,
    multiple: true,
    onDrop: (acceptedFiles) => {
      if (files.length >= maxFiles) { return; }
      const newFiles =
      acceptedFiles.filter((af) => files.findIndex((f) => f.name === af.name && f.size === af.size) === -1);
      if (newFiles.length === 0) { return; }
      if (newFiles.length + files.length > maxFiles) {
        const oversizedBy = (files.length + newFiles.length) - maxFiles;

        for (let idx = 0; idx < oversizedBy; idx++) {
          newFiles.pop();
        }
      }

      // tslint:disable-next-line: prefer-object-spread
      const mappedFiles = newFiles.map((file) => Object.assign(file, {preview: URL.createObjectURL(file)}));

      const newFilesArray = [...files, ...mappedFiles];
      setFiles(newFilesArray);
      onDrop(newFilesArray);
    },
  });

  useEffect(() => () => {
    files.forEach((file) => URL.revokeObjectURL(file.preview));
  }, [files]);

  const removeImage = (name: string, size: number) =>
    () => setFiles(files.filter((f) => f.name !== name && f.size !== size));

  const thumbs = files.map((file) => (
    <div className={Styles.thumb} key={file.name}>
      <div className={Styles.thumbInner} key={file.name}>
        <img
          onClick={removeImage(file.name, file.size)}
          src={file.preview}
          className={Styles.img}
          key={file.name}
        />
      </div>
    </div>
  ));

  return (
    <section className={joined(Styles.dropzoneContainer, className)}>
      <div {...getRootProps({className: Styles.dropzone})}>
        <input {...getInputProps()} />
        <p>{text}</p>
      </div>
      <aside className={Styles.thumbsContainer}>
        {thumbs}
      </aside>
    </section>
  );
}

export default FlexDropzone;
