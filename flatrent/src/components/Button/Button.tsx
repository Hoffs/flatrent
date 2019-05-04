import React, { ReactNode } from "react";
import Styles from "./Button.module.css";
import { Link } from "react-router-dom";

const Button = ({
    className = "",
    onClick,
    children,
    disabled,
    outline,
    to,
}: {
    className?: string;
    onClick?: () => void;
    children: ReactNode;
    disabled?: boolean;
    outline?: boolean;
    to?: string;
}) => {
    const button = (
        <button
            disabled={disabled}
            className={
                outline === true ? Styles.buttonOutline.concat(" ", className) : Styles.button.concat(" ", className)
            }
            onClick={onClick}
        >
            {children}
        </button>
    );
    if (to !== undefined) {
        return (
            <Link className={Styles.link} to={to}>
                {button}
            </Link>
        );
    } else {
        return button;
    }
};

export default Button;
