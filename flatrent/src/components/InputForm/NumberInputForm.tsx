import React, { Component } from "react";
import InputForm from "./InputForm";
import Styles from "./InputForm.module.css";

interface INumberInputFormProps {
  className?: string;
  errors?: string[];
  minValue?: number;
  maxValue?: number;
  value: number | string;
  name: string;
  title: string;
  errorsOnly?: boolean;
  setValue: (name: string, newValue: string) => void;
  extraProps?: { [key: string]: string };
}

class NumberInputForm extends Component<INumberInputFormProps, { focused: boolean }> {
  constructor(props: Readonly<INumberInputFormProps>) {
    super(props);
    this.state = { focused: false };
  }

  public render() {
    const { className, errors, errorsOnly, extraProps, name, title } = this.props;
    return (
      <InputForm
        className={className}
        name={name}
        errors={errors}
        errorsOnly={errorsOnly}
        title={title}
        extraProps={extraProps}
        type={"number"}
        value={this.props.value.toString()}
        setValue={this.checkAndSet}
      />
    );
  }

  private checkAndSet = (name: string, newValue: string) => {
    let newNumber = 0;
    newNumber = Number.parseInt(newValue, 10);
    if (isNaN(newNumber)) {
      return;
    }

    const { maxValue = Number.MAX_SAFE_INTEGER, minValue = Number.MIN_SAFE_INTEGER } = this.props;
    if (newNumber < minValue) {
      newNumber = minValue;
    }
    if (newNumber > maxValue) {
      newNumber = maxValue;
    }
    this.props.setValue(name, newNumber.toString());
  };
}

export default NumberInputForm;
