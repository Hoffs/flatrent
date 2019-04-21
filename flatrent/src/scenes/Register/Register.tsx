import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import Card from "../../components/Card";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import {InputForm} from "../../components/InputForm";
import UserService from "../../services/UserService";
import { register } from "../../serviceWorker";
import Styles from "./Register.module.css";

interface IRegisterState {
  values: {
    [key: string]: string;
    firstName: string;
    lastName: string;
    phone: string;
    email: string;
    emailConfirm: string;
    password: string;
    passwordConfirm: string;
  };
  requesting: boolean;
  errors: { [key: string]: string[] };
}

class Register extends Component<RouteComponentProps, IRegisterState> {
  constructor(props: RouteComponentProps) {
    super(props);
    this.state = {
      errors: {},
      requesting: false,
      values: {
        email: "",
        emailConfirm: "",
        firstName: "",
        lastName: "",
        password: "",
        passwordConfirm: "",
        phone: "",
      },
    };
  }

  public render() {
    return (
      <Card className={Styles.card}>
        <span className={Styles.title}>Registracija</span>
        <FlexRow>
          <InputForm
            value={this.state.values.firstName}
            errors={this.state.errors.FirstName}
            name="firstName"
            title="Vardas"
            setValue={this.handleUpdate}
          />
          <InputForm value={this.state.values.lastName} errors={this.state.errors.LastName} name="lastName" title="Pavardė" setValue={this.handleUpdate} />
        </FlexRow>
        <FlexRow>
          <InputForm
          value={this.state.values.phone}
            errors={this.state.errors.PhoneNumber}
            name="phone"
            title="Tel. Numeris"
            setValue={this.handleUpdate}
          />
        </FlexRow>
        <FlexRow>
          <InputForm value={this.state.values.email} errors={this.state.errors.Email} name="email" title="El. paštas" setValue={this.handleUpdate} />
          <InputForm
          value={this.state.values.emailConfirm}
            errors={this.state.errors.Email}
            name="emailConfirm"
            title="Pakartoti el. paštą"
            setValue={this.handleUpdate}
          />
        </FlexRow>
        <FlexRow>
          <InputForm
          value={this.state.values.password}
            errors={this.state.errors.Password}
            name="password"
            title="Slaptažodis"
            type="password"
            setValue={this.handleUpdate}
          />
          <InputForm
          value={this.state.values.passwordConfirm}
            errors={this.state.errors.Password}
            name="passwordConfirm"
            title="Pakartoti slaptažodį"
            type="password"
            setValue={this.handleUpdate}
          />
        </FlexRow>
        <Button disabled={this.state.requesting} onClick={this.register}>
          Registruotis
        </Button>
      </Card>
    );
  }

  private register = async () => {
    const { firstName, lastName, phone, password, passwordConfirm, email, emailConfirm } = this.state.values;

    const errors: { [key: string]: string[] } = {};
    if (password !== passwordConfirm) {
      errors.Password = ["Slaptažodžiai turi būti vienodi."];
    }
    if (email !== emailConfirm) {
      errors.Email = ["El. pašto adresai turi būti vienodi."];
    }
    if (Object.keys(errors).length > 0) {
      this.setState({ errors });
      return;
    }

    try {
      this.setState({ requesting: true });
      const result = await UserService.register({ email, firstName, lastName, password, phoneNumber: phone });
      if (Object.keys(result).length === 0) {
        toast.success("Sėkmingai užsiregsitravote!", {
          position: toast.POSITION.BOTTOM_CENTER,
        });
        this.props.history.push("/login");
      } else {
        this.setState({ errors: result, requesting: false });
      }
    } catch {
      // General error
      this.setState({ requesting: false });
    }
  };

  private handleUpdate = (name: string, value: string) =>
    this.setState((state) => ({ values: { ...state.values, [name]: value } }));
}

export default Register;
