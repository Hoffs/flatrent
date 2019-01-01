import React, { ReactNode } from "react";
import Styles from "./Button.module.css";

const Button = ({
  className = "",
  onClick,
  children,
  disabled,
}: {
  className?: string;
  onClick: () => void;
  children: ReactNode;
  disabled?: boolean;
}) => (
  <button disabled={disabled} className={Styles.button.concat(" ", className)} onClick={onClick}>
    {children}
  </button>
);

export default Button;
