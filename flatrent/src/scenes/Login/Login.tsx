import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import Card from "../../components/Card";
import { InputForm } from "../../components/InputForm";
import UserService from "../../services/UserService";
import Styles from "./Login.module.css";

const fields = ["Email", "Password"];

interface ILoginState {
  Values: { [key: string]: string };
  errors: { [key: string]: string[] };
  Requesting: boolean;
}

class Login extends Component<RouteComponentProps, ILoginState> {
  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
    this.state = {
      errors: {},
      Requesting: false,
      Values: { Email: "", Password: "" },
    };
  }

  public render() {
    return (
      <Card className={Styles.customCard}>
        <span className={Styles.title}>Prisijungimas</span>
        <InputForm
          value={this.state.Values.Email}
          errors={this.state.errors.email}
          name="Email"
          title="El. Paštas"
          setValue={this.handleChange}
        />
        <InputForm
          value={this.state.Values.Password}
          errors={this.state.errors.password}
          name="Password"
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
        <Button disabled={this.state.Requesting} onClick={this.authenticate}>
          Prisijungti
        </Button>
      </Card>
    );
  }

  private handleChange = (name: string, value: string) =>
    this.setState({ Values: { ...this.state.Values, [name]: value } });

  private authenticate = async () => {
    this.setState({ Requesting: true });
    try {
      const response = await UserService.authenticate(this.state.Values.Email, this.state.Values.Password);
      console.log(response);
      if (response.errors === undefined) {
        toast.success("Sėkmingai prisijugėte!", {
          position: toast.POSITION.BOTTOM_CENTER,
        });
        this.props.history.push("/");
        console.log("success");
      } else {
        this.setState({ errors: response.errors, Requesting: false });
        console.log("error");
      }
    } catch {
      this.setState({ Requesting: false });
    }
  };
}

export default Login;
