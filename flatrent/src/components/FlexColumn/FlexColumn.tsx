import React, { ReactNode } from "react";
import Styles from "./FlexColumn.module.css";

const FlexColumn = ({
  className = "",
  children,
  onClick,
}: {
  onClick?: (evt: React.MouseEvent<HTMLDivElement, MouseEvent>) => void;
  className?: string;
  children: ReactNode;
}) => (
  <div onClick={onClick} className={Styles.column.concat(" ", className)}>
    {children}
  </div>
);

export default FlexColumn;
