import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import Card from "../../components/Card";
import { InputForm } from "../../components/InputForm";
import UserService from "../../services/UserService";
import Styles from "./Login.module.css";

interface ILoginState {
    values: { [key: string]: string };
    errors: { [key: string]: string[] };
    requesting: boolean;
}

class Login extends Component<RouteComponentProps, ILoginState> {
    constructor(props: Readonly<RouteComponentProps>) {
        super(props);
        this.state = {
            errors: {},
            requesting: false,
            values: { email: "", password: "" },
        };
    }

    public render() {
        return (
            <Card className={Styles.customCard}>
                <span className={Styles.title}>Prisijungimas</span>
                <InputForm
                    value={this.state.values.email}
                    errors={this.state.errors.email}
                    name="email"
                    title="El. Paštas"
                    setValue={this.handleChange}
                />
                <InputForm
                    value={this.state.values.password}
                    errors={this.state.errors.password}
                    name="password"
                    type="password"
                    title="Slaptažodis"
                    setValue={this.handleChange}
                />
                <InputForm
                    value={""}
                    errorsOnly={true}
                    errors={this.state.errors.general}
                    name=""
                    title=""
                    setValue={this.handleChange}
                />
                <Button disabled={this.state.requesting} onClick={this.authenticate}>
                    Prisijungti
                </Button>
            </Card>
        );
    }

    private handleChange = (name: string, value: string) =>
        this.setState({ values: { ...this.state.values, [name]: value } });

    private authenticate = async () => {
        this.setState({ requesting: true });
        try {
            const response = await UserService.authenticate(this.state.values.email, this.state.values.password);
            console.log(response);
            if (response.errors === undefined) {
                toast.success("Sėkmingai prisijugėte!", {
                    position: toast.POSITION.BOTTOM_CENTER,
                });
                this.props.history.push("/");
                console.log("success");
            } else {
                this.setState({ errors: response.errors, requesting: false });
                console.log("error");
            }
        } catch {
            this.setState({ requesting: false });
        }
    };
}

export default Login;
