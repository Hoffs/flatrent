import React, { Children, Component, ReactNode } from "react";
import { Link } from "react-router-dom";

const BASE_STYLE = "top-navbar__item";
const BASE_STYLE_SELECTED = "top-navbar__item--selected";

const LinkWithHighlight = (props: {link: string, currentUrl: string, children: ReactNode}) => {
    const style = (props.currentUrl === props.link) ? `${BASE_STYLE} ${BASE_STYLE_SELECTED}` : BASE_STYLE;
    return (
        <li className={style}>
            <Link to={props.link}>{props.children}</Link>
        </li>
    );
};

export default LinkWithHighlight;
