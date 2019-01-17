import React, { ReactNode } from "react";
import Styles from "./FlexRow.module.css";

const FlexRow = ({className = "", children}: { className?: string, children: ReactNode }) => (
  <div className={Styles.row.concat(" ", className)}>
    {children}
  </div>
);

export default FlexRow;
