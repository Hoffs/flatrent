import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../../../components/Button";
import { CompactDropzone } from "../../../../components/Dropzones";
import FlexColumn from "../../../../components/FlexColumn";
import FlexRow from "../../../../components/FlexRow";
import { InputAreaForm, InputForm, NumberInputForm } from "../../../../components/InputForm";
import SimpleCheckbox from "../../../../components/SimpleCheckbox";
import FlatService from "../../../../services/FlatService";
import { IImageDetails } from "../../../../services/interfaces/FlatServiceInterfaces";
import { flatUrl, joined } from "../../../../utilities/Utilities";
import Styles from "./FlatEdit.module.css";

interface ICreateFlatState {
    values: {
        [key: string]: string | boolean;
        name: string;
        features: string;
        price: string;
        isFurnished: boolean;
        description: string;
        tenantRequirements: string;
        minimumRentDays: string;
    };
    images: File[];
    oldImages: IImageDetails[];
    requesting: boolean;
    errors: { [key: string]: string[] };
}

class CreateFlat extends Component<RouteComponentProps<{ id: string }>, ICreateFlatState> {
    constructor(props: RouteComponentProps<{ id: string }>) {
        super(props);
        this.state = {
            values: {
                name: "",
                features: "",
                price: "",
                description: "",

                isFurnished: false,
                minimumRentDays: "",
                tenantRequirements: "",
            },
            images: [],
            oldImages: [],
            requesting: true,
            errors: {},
        };
        this.fetchFlat();
    }

    public render() {
        const { errors } = this.state;
        return (
            <FlexColumn className={Styles.content}>
                <span className={Styles.title}>Redaguoti buto įrašą</span>
                <InputForm
                    value={""}
                    errors={errors.general}
                    errorsOnly={true}
                    name=""
                    title=""
                    setValue={this.handleUpdate}
                />
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

                <InputAreaForm
                    className={Styles.descriptionArea}
                    errors={errors.description}
                    name="description"
                    title="Aprašymas"
                    value={this.state.values.description}
                    setValue={this.handleUpdate}
                    maxChars={10240}
                />

                <InputAreaForm
                    className={Styles.requirementsArea}
                    errors={errors.tenantRequirements}
                    name="tenantRequirements"
                    title="Reikalavimai nuomininkui"
                    value={this.state.values.tenantRequirements}
                    setValue={this.handleUpdate}
                    maxChars={5120}
                />

                <InputForm errorsOnly={true} errors={errors.images} />
                <CompactDropzone
                    className={Styles.dropzone}
                    accept={["image/png", "image/jpg", "image/jpeg"]}
                    maxSize={5000000}
                    text="Norint pridėti nuotraukas nutempkitę jas į šį kvadratą arba jį paspauskite.
          Leistini formatai: png, jpg, jpeg.
          Maksimalus vienos nuotraukos dydis: 5 MB.
          (32 maks.)"
                    maxFiles={32}
                    onDrop={this.handleImageUpdate}
                    files={this.state.images}
                />

                <FlexRow className={Styles.buttonRow}>
                    <Button className={Styles.button} disabled={this.state.requesting} onClick={this.updateFlat}>
                        Atnaujinti
                    </Button>
                    <Button to={flatUrl(this.props.match.params.id)} className={Styles.button}>
                        Atšaukti
                    </Button>
                </FlexRow>
            </FlexColumn>
        );
    }

    private fetchFlat = async () => {
        const response = await FlatService.getFlat(this.props.match.params.id);
        if (response.errors !== undefined) {
            toast.error("Įvyko nežinoma klaida!", {
                position: toast.POSITION.BOTTOM_CENTER,
            });
            this.props.history.push(flatUrl(this.props.match.params.id));
        } else if (response.data !== undefined) {
            const files: File[] = response.data!.images.map((i) => ({
                name: i.name,
                lastModified: 1,
                size: 1,
                type: " ",
                slice: () =>
                    new Blob(undefined),
            }));

            this.setState((state) => ({
                values: {
                    ...state.values,
                    description: response.data!.description,
                    isFurnished: response.data!.isFurnished,
                    features: response.data!.features.join(", "),
                    name: response.data!.name,
                    price: response.data!.price.toString(),
                    minimumRentDays: response.data!.minimumRentDays.toString(),
                    tenantRequirements: response.data!.tenantRequirements,
                },
                images: files,
                requesting: false,
                oldImages: response.data!.images,
            }));
        }
    };

    private updateFlat = async () => {
        try {
            this.setState({ requesting: true });
            const response = await FlatService.updateFlat(
                this.props.match.params.id,
                { ...this.state.values },
                this.state.images,
                this.state.oldImages
            );

            console.log(response);
            if (response.data !== undefined) {
                toast.success("Sėkmingai atnaujintas įrašas!", {
                    position: toast.POSITION.BOTTOM_CENTER,
                });
                this.props.history.push(flatUrl(response.data.id));
            } else if (response.errors !== undefined) {
                this.setState({ errors: response.errors, requesting: false });
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

    private handleImageUpdate = (images: File[]) => this.setState({ images });
}

export default CreateFlat;
