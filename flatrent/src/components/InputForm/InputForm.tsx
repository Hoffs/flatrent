import React, { ChangeEvent, Component } from "react";
import { joined } from "../../utilities/Utilities";
import Styles from "./InputForm.module.css";

interface IInputFormProps {
    className?: string;
    type?: string;
    errors?: string[];
    name?: string;
    title?: string;
    value?: string;
    errorsOnly?: boolean;
    setValue?: (name: string, newValue: string) => void;
    onEnter?: () => void;
    extraProps?: { [key: string]: string };
}

class InputForm extends Component<IInputFormProps, { focused: boolean }> {
    constructor(props: Readonly<IInputFormProps>) {
        super(props);
        this.state = { focused: false };
    }

    public render() {
        return (
            <div className={joined(Styles.form, this.props.className ? this.props.className : "")}>
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
                        value={this.props.value}
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
        return this.state.focused || (typeof this.props.value !== undefined && this.props.value)
            ? joined(Styles.title, Styles.titleFocus)
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
        if (this.props.setValue !== undefined) {
            this.props.setValue(event.target.name, event.target.value);
        }
    };

    private onFocus = (event: React.FocusEvent<HTMLInputElement>) => {
        this.setState({ focused: event.type === "focus" ? true : false });
    };
}

export default InputForm;
