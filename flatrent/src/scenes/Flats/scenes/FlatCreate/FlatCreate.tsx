import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../../../components/Button";
import FlexColumn from "../../../../components/FlexColumn";
import FlexRow from "../../../../components/FlexRow";
import { InputForm, NumberInputForm, InputAreaForm } from "../../../../components/InputForm";
import SimpleCheckbox from "../../../../components/SimpleCheckbox";
import FlatService from "../../../../services/FlatService";
import Styles from "./FlatCreate.module.css";
import { joined, flatUrl } from "../../../../utilities/Utilities";
import FlexDropzone from "../../../../components/FlexDropzone";
import { IPreviewFile } from "../../../../components/FlexDropzone/FlexDropzone";
import { IFlatCreateResponse } from "../../../../services/interfaces/FlatServiceInterfaces";
import { IBasicResponse } from "../../../../services/interfaces/Common";

interface ICreateFlatState {
  values: {
    [key: string]: string | boolean;
    name: string;
    area: string;
    floor: string;
    roomCount: string;
    features: string;
    price: string;
    yearOfConstruction: string;

    isFurnished: boolean;

    description: string;

    tenantRequirements: string;
    minimumRentDays: string;

    street: string;
    houseNumber: string;
    flatNumber: string;
    city: string;
    country: string;
    postCode: string;
  };
  images: File[];
  requesting: boolean;
  errors: { [key: string]: string[] };
}

class CreateFlat extends Component<RouteComponentProps, ICreateFlatState> {
  constructor(props: RouteComponentProps) {
    super(props);
    this.state = {
      values: {
        area: "",
        name: "",
        floor: "",
        totalFloors: "",
        roomCount: "",
        features: "",
        price: "",
        yearOfConstruction: "",
        description: "",

        isFurnished: false,
        minimumRentDays: "",
        tenantRequirements: "",

        street: "",
        houseNumber: "",
        flatNumber: "",
        city: "",
        country: "",
        postCode: "",
      },
      images: [],
      requesting: false,
      errors: {},
    };
  }

  public render() {
    const { errors } = this.state;
    return (
      <FlexColumn className={Styles.content}>
        <span className={Styles.title}>Sukurti naują buto įrašą</span>
        <InputForm value={""} errors={errors.general} errorsOnly={true} name="" title="" setValue={this.handleUpdate} />
        <FlexRow>
          <InputForm
            className={Styles.input}
            value={this.state.values.name}
            errors={errors.name}
            name="name"
            title="Pavadinimas"
            setValue={this.handleUpdate}
          />
        </FlexRow>
        <FlexRow>
          <NumberInputForm
            className={Styles.input}
            minValue={0}
            maxValue={2050}
            value={this.state.values.yearOfConstruction}
            errors={errors.yearOfConstruction}
            name="yearOfConstruction"
            title="Statybos metai"
            setValue={this.handleUpdate}
          />
          <NumberInputForm
            className={Styles.input}
            minValue={0}
            maxValue={100}
            value={this.state.values.floor}
            errors={errors.floor}
            name="floor"
            title="Aukštas"
            setValue={this.handleUpdate}
          />
          <NumberInputForm
            className={Styles.input}
            minValue={0}
            maxValue={100}
            value={this.state.values.totalFloors.toString()}
            errors={errors.totalFloors}
            name="totalFloors"
            title="Aukštų skaičius"
            setValue={this.handleUpdate}
          />
        </FlexRow>
        <FlexRow>
          <NumberInputForm
            className={Styles.input}
            minValue={1}
            maxValue={128}
            value={this.state.values.roomCount}
            errors={errors.roomCount}
            name="roomCount"
            title="Kambarių skaičius"
            setValue={this.handleUpdate}
          />
          <NumberInputForm
            className={Styles.input}
            minValue={1}
            maxValue={512}
            value={this.state.values.area}
            errors={errors.area}
            name="area"
            title="Plotas"
            setValue={this.handleUpdate}
          />
          <SimpleCheckbox
            className={joined(Styles.furnishedCheckbox, Styles.input)}
            name="isFurnished"
            size={28}
            checked={this.state.values.isFurnished}
            setValue={this.handleUpdate}
          >
            Įrengtas
          </SimpleCheckbox>
        </FlexRow>
        <InputForm
          className={Styles.input}
          value={this.state.values.features.toString()}
          errors={errors.features}
          name="features"
          title="Ypatybės (atskirti kableliu)"
          setValue={this.handleUpdate}
        />
        <FlexRow>
          <NumberInputForm
            className={Styles.input}
            minValue={1}
            maxValue={10000}
            value={this.state.values.price}
            errors={errors.price}
            name="price"
            title="Kaina (Eurų per mėnesį)"
            setValue={this.handleUpdate}
          />
          <NumberInputForm
            className={Styles.input}
            minValue={1}
            maxValue={3650}
            value={this.state.values.minimumRentDays}
            errors={errors.minimumRentDays}
            name="minimumRentDays"
            title="Minimali nuoma dienomis"
            setValue={this.handleUpdate}
          />
        </FlexRow>

        <span className={Styles.section}>Buto adresas:</span>
        <InputForm
          className={Styles.input}
          value={this.state.values.country}
          errors={errors.country}
          name="country"
          title="Šalis"
          setValue={this.handleUpdate}
        />
        <FlexRow>
          <InputForm
            className={Styles.input}
            value={this.state.values.city}
            errors={errors.city}
            name="city"
            title="Miestas"
            setValue={this.handleUpdate}
          />
          <InputForm
            className={Styles.input}
            value={this.state.values.postCode}
            errors={errors.postCode}
            name="postCode"
            title="Pašto kodas"
            setValue={this.handleUpdate}
          />
        </FlexRow>

        <InputForm
          className={Styles.input}
          value={this.state.values.street}
          errors={errors.street}
          name="street"
          title="Gatvė"
          setValue={this.handleUpdate}
        />
        <FlexRow>
          <InputForm
            className={Styles.input}
            value={this.state.values.houseNumber}
            errors={errors.houseNumber}
            name="houseNumber"
            title="Namo numeris"
            setValue={this.handleUpdate}
          />
          <InputForm
            className={Styles.input}
            value={this.state.values.flatNumber}
            errors={errors.flatNumber}
            name="flatNumber"
            title="Buto numeris"
            setValue={this.handleUpdate}
          />
        </FlexRow>

        <InputAreaForm
          className={Styles.descriptionArea}
          errors={errors.description}
          name="description"
          title="Aprašymas"
          setValue={this.handleUpdate}
        />

        <InputAreaForm
          className={Styles.requirementsArea}
          errors={errors.tenantRequirements}
          name="tenantRequirements"
          title="Reikalavimai nuomininkui"
          setValue={this.handleUpdate}
        />

        <InputForm errorsOnly={true} errors={errors.images} />
        <FlexDropzone
          className={Styles.dropzone}
          accept={["image/png", "image/jpg", "image/jpeg"]}
          maxSize={5000000}
          text="Norint pridėti nuotraukas nutempkitę jas į šį kvadratą arba jį paspauskite.
          Leistini formatai: png, jpg, jpeg.
          Maksimalus vienos nuotraukos dydis: 5 MB.
          Leistinos 32 nuotraukos."
          maxFiles={32}
          onDrop={this.handleImageUpdate}
        />

        <FlexRow className={Styles.buttonRow}>
          <Button className={Styles.button} disabled={this.state.requesting} onClick={this.publishFlat}>
            Publikuoti
          </Button>
          <Button className={Styles.button} disabled={this.state.requesting} onClick={this.saveFlat}>
            Išsaugoti
          </Button>
        </FlexRow>
      </FlexColumn>
    );
  }

  private saveFlat = async () => this.createFlat(false);
  private publishFlat = async () => this.createFlat(true);

  private createFlat = async (publish: boolean) => {
    try {
      this.setState({ requesting: true });
      const response = await FlatService.createFlat({ ...this.state.values, isPublished: publish }, this.state.images);

      if ((response as IFlatCreateResponse).id !== undefined) {
        toast.success("Sėkmingai sukurtas įrašas!", {
          position: toast.POSITION.BOTTOM_CENTER,
        });
        const { id } = response as IFlatCreateResponse;
        this.props.history.push(flatUrl(id));
      } else if ((response as IBasicResponse).errors !== undefined) {
        const { errors = {} } = response as IBasicResponse;
        this.setState({ errors, requesting: false });
      }
    } catch (error) {
      console.log(error);
      toast.error("Įvyko nežinoma klaida!", {
        position: toast.POSITION.BOTTOM_CENTER,
      });
      this.setState({ requesting: false });
    }
  };

  private handleUpdate = (name: string, value: string | boolean) =>
    this.setState((state) => ({ values: { ...state.values, [name]: value } }));

  private handleImageUpdate = (images: IPreviewFile[]) => this.setState({ images });
}

export default CreateFlat;
