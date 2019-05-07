import React, { ReactNode } from "react";
import onClickOutside from "react-onclickoutside";
import { RouteComponentProps } from "react-router-dom";
import Button from "../../../components/Button";
import { InputForm } from "../../../components/InputForm";
import { joined } from "../../../utilities/Utilities";
import Styles from "./FilterItem.module.css";

interface ITextFilterItemProps extends RouteComponentProps {
    title: string;
    name: string;
    children?: ReactNode;
    onQueryUpdate: (queryString: string) => void;
}

interface ITextFilterItemsState {
    value: string;
    isOpen: boolean;
    toRight: boolean;
}

class TextFilterItem extends React.Component<ITextFilterItemProps, ITextFilterItemsState> {
    public state = { value: "", isOpen: false, toRight: false };
    constructor(props: ITextFilterItemProps) {
        super(props);
        const params = new URLSearchParams(props.location.search);
        const value = params.get(props.name);
        if (value !==  null) {
            try {
                this.state.value = value;
            } catch { }
        }
    }

    public handleClickOutside = () => this.setState({ isOpen: false });

    public render() {
        const props = this.props;

        const dimmerStyle = !this.state.isOpen ? joined(Styles.dimmer, Styles.closed) : Styles.dimmer;
        const wrapperStyle = !this.state.isOpen
            ? joined(Styles.popupWrapper, Styles.closed)
            : (!this.state.toRight)
                ? Styles.popupWrapper
                : joined(Styles.popupWrapper, Styles.rightAlign);
        const label = this.state.value === "" ? props.title : `${props.title}, ${this.state.value}`;
        return (
            <div className={Styles.item}>
                <button className={Styles.nameButton} onClick={this.toggle}>
                    {label}
                </button>
                <div className={wrapperStyle}>
                    {/* {props.children} */}
                    <InputForm
                        name={this.props.name}
                        title={this.props.title}
                        value={this.state.value}
                        setValue={this.setValue}
                    />
                    <Button className={Styles.popupButton} onClick={this.confirm}>Patvirtinti</Button>
                </div>
                <div className={dimmerStyle} onClick={this.handleClickOutside} />
            </div>
        );
    }

    private toggle = (evt: React.MouseEvent<HTMLButtonElement>) => {
        const bt = evt.target as HTMLButtonElement;
        this.setState((state) => ({
            isOpen: !state.isOpen,
            toRight: bt.getBoundingClientRect().left + 250 > window.innerWidth,
        }))
    };
    private confirm = () => this.setState({ isOpen: false });
    private setValue = (_: string, val: string) => {
        this.setState({ value: val });
        this.props.onQueryUpdate(`${this.props.name}=${val.toString()}`);
    }
}

export default onClickOutside(TextFilterItem);
