import React, { ChangeEvent, Component } from "react";
import Styles from "./InputForm.module.css";

interface InputAreaFormProps {
    className?: string;
    type?: string;
    errors?: string[];
    name: string;
    title: string;
    maxChars?: number;
    errorsOnly?: boolean;
    setValue: (name: string, value: string) => void;
    extraProps?: { [key: string]: string };
    value: string;
}

class InputAreaForm extends Component<InputAreaFormProps, { focused: boolean }> {
    constructor(props: Readonly<InputAreaFormProps>) {
        super(props);
        this.state = { focused: false };
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
        const charCounter =
            this.props.maxChars !== undefined ? (
                <span className={Styles.charCounter}>
                    {this.props.value.length} / {this.props.maxChars}
                </span>
            ) : (
                undefined
            );
        if (!this.props.errorsOnly) {
            const style = this.props.className === undefined ? "" : this.props.className;
            return (
                <>
                    <span className={this.getTitleStyle()}>{this.props.title}</span>
                    <textarea
                        className={Styles.input.concat(" ", style)}
                        name={this.props.name}
                        value={this.props.value}
                        onChange={this.handleChange}
                        onFocus={this.onFocus}
                        onBlur={this.onFocus}
                        {...this.props.extraProps}
                    />
                    {charCounter}
                </>
            );
        }
        return null;
    }

    private getTitleStyle() {
        return this.state.focused || (typeof this.props.value !== undefined && this.props.value)
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
        this.props.setValue(event.target.name, event.target.value);
    };

    private onFocus = (event: React.FocusEvent<HTMLTextAreaElement>) => {
        this.setState({ focused: event.type === "focus" ? true : false });
    };
}

export default InputAreaForm;
