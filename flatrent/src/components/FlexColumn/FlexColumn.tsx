import React, { ReactNode } from "react";
import Styles from "./FlexColumn.module.css";

const FlexColumn = ({className = "", children}: { className?: string, children: ReactNode }) => (
  <div className={Styles.column.concat(" ", className)}>
    {children}
  </div>
);

export default FlexColumn;
