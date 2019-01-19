import React, { Component } from "react";
import Card from "../../components/Card";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import InputForm from "../../components/InputForm";
import Styles from "./FlatCreate.module.css";
import Button from "../../components/Button";
import FlatService from "../../services/FlatService";
import { toast } from "react-toastify";
import { RouteComponentProps } from "react-router-dom";
import InputArea from "../../components/InputArea";

interface IRegisterState {
  values: {
    [key: string]: string;
    name: string;
    area: string;
    floor: string;
    roomCount: string;
    price: string;
    yearOfConstruction: string;
    description: string;

    ownerName: string;
    account: string;
    email: string;
    phoneNumber: string;

    street: string;
    houseNumber: string;
    flatNumber: string;
    city: string;
    country: string;
    postCode: string;
  };
  requesting: boolean;
  errors: { [key: string]: string[] };
}

class CreateFlat extends Component<RouteComponentProps, IRegisterState> {
  constructor(props: RouteComponentProps) {
    super(props);
    this.state = {
      values: {
        area: "",
        name: "",
        floor: "",
        roomCount: "",
        price: "",
        yearOfConstruction: "",
        description: "",

        ownerName: "",
        account: "",
        email: "",
        phoneNumber: "",

        street: "",
        houseNumber: "",
        flatNumber: "",
        city: "",
        country: "",
        postCode: "",
      },
      requesting: false,
      errors: {},
    };
  }

  public render() {
    const { errors } = this.state;
    return (
      <Card className={Styles.card}>
        <span className={Styles.title}>Sukurti naują buto įrašą</span>
        <InputForm errors={errors.General} errorsOnly={true} name="" title="" setValue={this.handleUpdate} />
        <FlexRow>
          <InputForm errors={errors.Name} name="name" title="Pavadinimas" setValue={this.handleUpdate} />
        </FlexRow>
        <FlexRow>
          <InputForm
            errors={errors.YearOfConstruction}
            name="yearOfConstruction"
            type="number"
            title="Statybos metai"
            setValue={this.handleUpdate}
          />
          <InputForm errors={errors.Floor} name="floor" type="number" title="Aukštas" setValue={this.handleUpdate} />
        </FlexRow>
        <FlexRow>
          <InputForm
            errors={errors.RoomCount}
            name="roomCount"
            type="number"
            title="Kambarių skaičius"
            setValue={this.handleUpdate}
          />
          <InputForm errors={errors.Area} name="area" type="number" title="Plotas" setValue={this.handleUpdate} />
        </FlexRow>
        <InputForm errors={errors.Price} name="price" type="number" title="Kaina" setValue={this.handleUpdate} />

        <span className={Styles.section}>Adresas:</span>
        <InputForm errors={errors.Street} name="street" title="Gatvė" setValue={this.handleUpdate} />
        <FlexRow>
          <InputForm errors={errors.HouseNumber} name="houseNumber" title="Namo numeris" setValue={this.handleUpdate} />
          <InputForm errors={errors.FlatNumber} name="flatNumber" title="Buto numeris" setValue={this.handleUpdate} />
        </FlexRow>
        <FlexRow>
          <InputForm errors={errors.City} name="city" title="Miestas" setValue={this.handleUpdate} />
          <InputForm errors={errors.PostCode} name="postCode" title="Pašto kodas" setValue={this.handleUpdate} />
        </FlexRow>
        <InputForm errors={errors.Country} name="country" title="Šalis" setValue={this.handleUpdate} />

        <span className={Styles.section}>Savininko informacija:</span>
        <FlexRow>
          <InputForm
            errors={errors.OwnerName}
            name="ownerName"
            title="Vardas/Pavadinimas"
            setValue={this.handleUpdate}
          />
          <InputForm errors={errors.Account} name="account" title="Sąskaita" setValue={this.handleUpdate} />
        </FlexRow>
        <FlexRow>
          <InputForm errors={errors.Email} name="email" type="email" title="El. Paštas" setValue={this.handleUpdate} />
          <InputForm
            errors={errors.PhoneNumber}
            name="phoneNumber"
            type="phone"
            title="Tel. Numeris"
            setValue={this.handleUpdate}
          />
        </FlexRow>
        <InputArea
          className={Styles.descriptionArea}
          errors={errors.Description}
          name="description"
          title="Aprašymas"
          setValue={this.handleUpdate}
        />
        <Button disabled={this.state.requesting} onClick={this.createFlat}>
          Sukurti
        </Button>
      </Card>
    );
  }

  private createFlat = async () => {
    try {
      this.setState({ requesting: true });
      const response = await FlatService.createFlat(this.state.values);
      if (response.errors === undefined) {
        toast.success("Sėkmingai sukurtas įrašas!", {
          position: toast.POSITION.BOTTOM_CENTER,
        });
        this.props.history.push("/flats");
      } else {
        this.setState({ errors: response.errors, requesting: false });
      }
    } catch (error) {
      toast.error("Įvyko nežinoma klaida!", {
        position: toast.POSITION.BOTTOM_CENTER,
      });
      this.setState({ requesting: false });
    }
  };

  private handleUpdate = (name: string, value: string) =>
    this.setState((state) => ({ values: { ...state.values, [name]: value } }));
}

export default CreateFlat;
