import React, { useEffect, useState } from "react";
import { useDropzone } from "react-dropzone";
import { joined } from "../../utilities/Utilities";
import Styles from "./ImageDropzone.module.css";

export interface IImageDropzone {
    onDrop: (acceptedFiles: IPreviewFile[]) => void;
    minSize?: number;
    maxSize?: number;
    maxFiles?: number;
    accept?: string | string[];
    text: string;
    className?: string;
    initialFiles?: File[];
}

export interface IPreviewFile extends File {
    preview: string;
}

function ImageDropzone({
    text,
    className,
    onDrop,
    minSize,
    maxSize,
    maxFiles = 8,
    accept,
    initialFiles = [],
}: IImageDropzone) {
    const mappedFiles = initialFiles.map((file) => Object.assign(file, { preview: URL.createObjectURL(file) }));
    const [files, setFiles] = useState<IPreviewFile[]>(mappedFiles);

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

            // tslint:disable-next-line: prefer-object-spread
            const mappedFiles = newFiles.map((file) => Object.assign(file, { preview: URL.createObjectURL(file) }));

            const newFilesArray = [...files, ...mappedFiles];
            setFiles(newFilesArray);
            onDrop(newFilesArray);
        },
    });

    useEffect(
        () => () => {
            files.forEach((file) => URL.revokeObjectURL(file.preview));
        },
        [files]
    );

    const removeItem = (name: string) => () => {
        const leftFiles = files.filter((f) => f.name !== name);
        setFiles(leftFiles);
        onDrop(leftFiles);
    };

    const getThumbContent = (file: IPreviewFile) => {
        if (file.type.startsWith("image")) {
            return <img alt={file.name} src={file.preview} className={Styles.img} key={file.name} />;
        } else {
            return <span className={Styles.textThumb}>{file.name}</span>;
        }
    };

    const thumbs = files.map((file) => (
        <div onClick={removeItem(file.name)} className={Styles.thumb} key={file.name}>
            {getThumbContent(file)}
        </div>
    ));

    return (
        <section className={joined(Styles.dropzoneContainer, className)}>
            <div {...getRootProps({ className: Styles.dropzone })}>
                <input {...getInputProps()} />
                <p className={Styles.pText}>{text}</p>
            </div>
            <div className={Styles.thumbsContainer}>{thumbs}</div>
        </section>
    );
}

export default ImageDropzone;
