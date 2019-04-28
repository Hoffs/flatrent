import React, { ReactNode } from "react";
import Styles from "./Card.module.css";

const Card = ({ className = "", children }: { className?: string; children: ReactNode }) => (
  <div className={Styles.card.concat(" ", className)}>{children}</div>
);

export default Card;
