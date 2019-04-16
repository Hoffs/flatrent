import React, { Children, Component, ReactNode } from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import Styles from "./NavBar.module.css";

const BASE_STYLE = Styles.button;
const SELECTED_STYLE = Styles.buttonSelected;

interface INavigationButtonProps {
  link: string;
  currentUrl: string;
  children: ReactNode;
};

const NavigationButton = (props: INavigationButtonProps & RouteComponentProps) => {
    // props.history.push()

    const onClickOpen = () => props.history.push(props.link);

    const style = (props.currentUrl === props.link) ? SELECTED_STYLE : BASE_STYLE;
    return (
        <button onClick={onClickOpen} className={style}>
            <div className={Styles.buttonContent}>
                {props.children}
            </div>
            {/* <Link to={props.link}>{props.children}</Link> */}
        </button>
    );
};

export default withRouter(NavigationButton);
