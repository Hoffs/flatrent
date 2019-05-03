import React, { ReactNode } from "react";
import Styles from "./Dimmer.module.css";

const Dimmer = ({
  children,
  onClick,
}: {
  children: ReactNode;
  onClick?: (event: React.MouseEvent<HTMLDivElement, MouseEvent>) => void;
}) => (
  <div onClick={onClick} className={Styles.dimmer}>
    {children}
  </div>
);

export default Dimmer;
