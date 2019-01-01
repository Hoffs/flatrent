import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import Card from "../../components/Card";
import InputForm from "../../components/InputForm";
import UserService from "../../services/UserService";
import Styles from "./Login.module.css";

const fields = ["Email", "Password"];

interface ILoginState {
  Values: { [key: string]: string };
  Errors: { [key: string]: string[] };
}

class Login extends Component<RouteComponentProps, ILoginState> {
  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
    this.state = {
      Errors: {},
      Values: { Email: "", Password: "" },
    };
  }

  public render() {
    return (
      <Card className={Styles.customCard}>
        <span className={Styles.title}>Prisijungti</span>
        <InputForm errors={this.state.Errors.Email} name="Email" title="El. Paštas" setValue={this.handleChange} />
        <InputForm
          errors={this.state.Errors.Password}
          name="Password"
          type="password"
          title="Slaptažodis"
          setValue={this.handleChange}
        />
        <InputForm errorsOnly={true} errors={this.state.Errors.General} name="" title="" setValue={this.handleChange} />
        <Button onClick={this.authenticate}>Prisijungti</Button>
      </Card>
    );
  }

  private handleChange = (name: string, value: string) =>
    this.setState({ Values: { ...this.state.Values, [name]: value } });

  private authenticate = async () => {
    const errors = await UserService.authenticate(this.state.Values.Email, this.state.Values.Password);
    console.log(errors);
    if (Object.keys(errors).length === 0) {
      toast.success("Sėkmingai prisijugėte!", {
        position: toast.POSITION.BOTTOM_CENTER,
      });
      setTimeout(() => this.props.history.push("/"), 1000);
      console.log("success");
    } else {
      this.setState({ Errors: errors });
      console.log("error");
    }
  };
}

export default Login;
