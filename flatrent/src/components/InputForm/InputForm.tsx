import React, { ChangeEvent, Component } from "react";
import Styles from "./InputForm.module.css";

interface InputFormProps {
  type?: string;
  default?: string;
  errors?: string[];
  name: string;
  title: string;
  errorsOnly?: boolean;
  setValue: (name: string, newValue: string) => void;
}

class InputForm extends Component<InputFormProps, { value: string; focused: boolean }> {
  constructor(props: Readonly<InputFormProps>) {
    super(props);
    this.state = { value: "", focused: false };
  }

  public render() {
    return (
      <div className={Styles.form}>
        {this.getContent()}
        {this.getErrors()}
      </div>
    );
  }

  private getContent() {
    if (!this.props.errorsOnly) {
      return (
        <>
          <span className={this.getTitleStyle()}>{this.props.title}</span>
          <input
            className={Styles.input}
            type={this.props.type === undefined ? "text" : this.props.type}
            name={this.props.name}
            defaultValue={this.props.default}
            value={this.state.value}
            onChange={this.handleChange}
            onFocus={this.onFocus}
            onBlur={this.onFocus}
          />
        </>
      );
    }
    return null;
  }

  private getTitleStyle() {
    return this.state.focused || (typeof this.state.value !== undefined && this.state.value)
      ? Styles.title.concat(" ", Styles.titleFocus)
      : Styles.title;
  }

  private getErrors() {
    if (this.props.errors === undefined || this.props.errors.length === 0) {
      return null;
    }
    return this.props.errors.map((error, index) => (
      <span key={index} className={Styles.error}>
        {error}
      </span>
    ));
  }

  private handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    this.setState({ value: event.target.value });
    this.props.setValue(event.target.name, event.target.value);
  };

  private onFocus = (event: React.FocusEvent<HTMLInputElement>) => {
    this.setState({ focused: event.type === "focus" ? true : false });
  };
}

export default InputForm;
