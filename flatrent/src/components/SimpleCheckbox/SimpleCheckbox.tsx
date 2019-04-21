import React, { ComponentProps, ReactNode, ChangeEvent } from "react";
import Styles from "./SimpleCheckbox.module.css";
import { joined } from "../../utilities/Utilities";

export interface ISimpleCheckboxProps {
  className?: string;
  checked: boolean;
  size?: number;
  children: ReactNode;
  setValue: (name: string, newValue: boolean) => void;
}

const changeAndCall = (
  setValue: (name: string, newValue: boolean) => void
): ((evt: ChangeEvent<HTMLInputElement>) => void) => (ev) => {
  console.log(ev.target.name, ev.target.value);
  setValue(ev.target.name, ev.target.checked);
};

const SimpleCheckbox = <P extends ISimpleCheckboxProps & ComponentProps<"input">>({
  className,
  size = 16,
  checked,
  setValue,
  children,
  name,
}: P) => (
  <label className={joined(Styles.label, className ? className : "")}>
    <div className={Styles.container}>
      <input
        className={Styles.hiddenCheckbox}
        type={"checkbox"}
        checked={checked}
        name={name}
        onChange={changeAndCall(setValue)}
      />
      <div style={{ width: size, height: size }} className={Styles.styledCheckbox} x-checked={checked.toString()}>
        <svg x-checked={checked.toString()} className={Styles.icon} viewBox="0 0 24 24">
          <polyline points="20 6 9 17 4 12" />
        </svg>
      </div>
    </div>
    <span>{children}</span>
  </label>
);

export default SimpleCheckbox;
