import React, { Children, Component, ReactNode } from "react";
import { Link } from "react-router-dom";
import Styles from "./NavBar.module.css";

const BASE_STYLE = Styles.navbarItem;
const BASE_STYLE_SELECTED = Styles.navbarItemSelected;

const LinkWithHighlight = (props: {link: string, currentUrl: string, children: ReactNode}) => {
    const style = (props.currentUrl === props.link) ? `${BASE_STYLE} ${BASE_STYLE_SELECTED}` : BASE_STYLE;
    return (
        <li className={style}>
            <Link to={props.link}>{props.children}</Link>
        </li>
    );
};

export default LinkWithHighlight;
