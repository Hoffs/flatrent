import React, { DetailedHTMLProps, ImgHTMLAttributes, SyntheticEvent } from "react";

const setImageToDefault = (evt: SyntheticEvent<HTMLImageElement, Event>) => {
    evt.currentTarget.setAttribute("src", "/placeholder.svg");
    evt.currentTarget.setAttribute("style", "height: auto;width: 80%; min-width: auto;min-height: auto;");
};

const SmartImg = <P extends DetailedHTMLProps<ImgHTMLAttributes<HTMLImageElement>, HTMLImageElement>>(props: P) => {
    return <img alt="" {...props} onError={setImageToDefault} />;
};

export default SmartImg;
