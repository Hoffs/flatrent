import React, { Component } from "react";
import Card from "../../components/Card";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import InputForm from "../../components/InputForm";
import Styles from "./FlatCreate.module.css";

interface IRegisterState {
  values: {
    [key: string]: string,
    fName: string,
    lName: string,
    phone: string,
    email: string,
    emailConfirm: string,
    password: string,
    passwordConfirm: string,
  };
}

class CreateFlat extends Component<{}, IRegisterState> {
  constructor(props: {}) {
    super(props);
    this.state = { values: {
        email: "",
        emailConfirm: "",
        fName: "",
        lName: "",
        password: "",
        passwordConfirm: "",
        phone: "",
      },
    };
  }

  public render() {
    return (
      <Card className={Styles.card}>
        <span>Registruotis</span>
        <FlexRow>
          <FlexColumn>
            <InputForm name="fName" title="Vardas" setValue={this.handleUpdate} />
            <InputForm name="phone" title="Tel. Numeris" setValue={this.handleUpdate} />
          </FlexColumn>
          <FlexColumn>
            <InputForm name="lName" title="Pavardė" setValue={this.handleUpdate} />
          </FlexColumn>
        </FlexRow>
        <FlexRow>
          <InputForm name="email" title="El. paštas" setValue={this.handleUpdate} />
          <InputForm name="emailConfirm" title="Pakartoti el. paštą" setValue={this.handleUpdate} />
        </FlexRow>
        <FlexRow>
          <InputForm name="password" title="Slaptažodis" type="password" setValue={this.handleUpdate} />
          <InputForm name="passwordConfirm" title="Pakartoti slaptažodį" type="password" setValue={this.handleUpdate} />
        </FlexRow>
      </Card>
    );
  }

  private handleUpdate = (name: string, value: string) =>
    this.setState({ values: {...this.state.values, [name]: value }})

}

export default CreateFlat;
