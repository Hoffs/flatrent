import React, { ChangeEvent } from "react";
import Styles from "./Checkbox.module.css";

interface ICheckboxProps {
  checked?: boolean;
  onChange: (state: boolean) => void;
  text: string;
}

const makeid = (size: number) => {
  let text = "";
  const possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

  for (let i = 0; i < size; i++) {
    text += possible.charAt(Math.floor(Math.random() * possible.length));
  }

  return text;
};

const Checkbox = (props: ICheckboxProps) => {
  const key = makeid(12);
  const onChange = (event: ChangeEvent<HTMLInputElement>) => props.onChange(event.target.checked);

  return (
    <div className={Styles.checkbox}>
      <input id={key} className={Styles.input} onChange={onChange} type="checkbox" defaultChecked={props.checked} />
      <label className={Styles.label} htmlFor={key}>
        {props.text}
      </label>
    </div>
  );
};

export default Checkbox;
