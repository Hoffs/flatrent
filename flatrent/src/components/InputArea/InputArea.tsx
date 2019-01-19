import React, { ChangeEvent, Component } from "react";
import Styles from "./InputArea.module.css";

interface InputAreaProps {
  className?: string;
  type?: string;
  default?: string;
  errors?: string[];
  name: string;
  title: string;
  errorsOnly?: boolean;
  setValue: (name: string, newValue: string) => void;
  extraProps?: { [key: string]: string };
}

class InputArea extends Component<InputAreaProps, { value: string; focused: boolean }> {
  constructor(props: Readonly<InputAreaProps>) {
    super(props);
    this.state = { value: props.default !== undefined ? props.default : "", focused: false };
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
      const style = this.props.className === undefined ? "" : this.props.className;
      return (
        <>
          <span className={this.getTitleStyle()}>{this.props.title}</span>
          <textarea
            className={Styles.input.concat(" ", style)}
            name={this.props.name}
            value={this.state.value}
            onChange={this.handleChange}
            onFocus={this.onFocus}
            onBlur={this.onFocus}
            {...this.props.extraProps}
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

  private handleChange = (event: ChangeEvent<HTMLTextAreaElement>) => {
    this.setState({ value: event.target.value });
    this.props.setValue(event.target.name, event.target.value);
  };

  private onFocus = (event: React.FocusEvent<HTMLTextAreaElement>) => {
    this.setState({ focused: event.type === "focus" ? true : false });
  };
}

export default InputArea;
