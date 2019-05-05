import React, { ReactNode } from "react";
import Styles from "./Button.module.css";
import { Link } from "react-router-dom";
import { join } from "path";
import { joined } from "../../utilities/Utilities";

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
            className={to === undefined ? joined(Styles.button, className) : Styles.button}
            onClick={onClick}
        >
            {children}
        </button>
    );
    if (to !== undefined) {
        return (
            <Link className={joined(Styles.link, className)} to={to}>
                {button}
            </Link>
        );
    } else {
        return button;
    }
};

export default Button;
