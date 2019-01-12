import React, { ReactNode } from "react";
import Styles from "./Button.module.css";

const Button = ({
  className = "",
  onClick,
  children,
  disabled,
  outline,
}: {
  className?: string;
  onClick: () => void;
  children: ReactNode;
  disabled?: boolean;
  outline?: boolean;
}) => (
  <button
    disabled={disabled}
    className={outline === true ? Styles.buttonOutline.concat(" ", className) : Styles.button.concat(" ", className)}
    onClick={onClick}
  >
    {children}
  </button>
);

export default Button;
