import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Styles from "./CreateIncidentModal.module.css";

import Moment from "moment";
// tslint:disable-next-line: no-submodule-imports
import "moment/locale/lt";
// tslint:disable-next-line: no-submodule-imports
import "react-dates/initialize";
// tslint:disable-next-line: no-submodule-imports
import "react-dates/lib/css/_datepicker.css";
import Button from "../../../components/Button";
import Dimmer from "../../../components/Dimmer";
import { ImageDropzone } from "../../../components/Dropzones";
import { IPreviewFile } from "../../../components/Dropzones/ImageDropzone";
import FlexColumn from "../../../components/FlexColumn";
import FlexRow from "../../../components/FlexRow";
import { InputAreaForm, InputForm } from "../../../components/InputForm";
import IncidentService from "../../../services/IncidentService";
import { IAgreementDetails } from "../../../services/interfaces/AgreementInterfaces";
import { IApiResponse, IErrorResponse, IInputValues } from "../../../services/interfaces/Common";
import { IFaultCreateResponse } from "../../../services/interfaces/FaultInterfaces";
import { agreementUrl, incidentUrl, stopPropogation } from "../../../utilities/Utilities";

Moment.locale("lt");

export interface ICreateIncidentModalProps {
    agreement: IAgreementDetails;
}

interface ICreateIncidentModalState {
    requesting: boolean;
    requestingPdf: boolean;
    values: IInputValues;
    files: File[];
    errors: IErrorResponse;
}

class CreateIncidentModal extends Component<
    RouteComponentProps<{ id: string; invoiceId: string }> & ICreateIncidentModalProps,
    ICreateIncidentModalState
> {
    constructor(props: RouteComponentProps<{ id: string; invoiceId: string }> & ICreateIncidentModalProps) {
        super(props);
        this.state = {
            requesting: false,
            requestingPdf: false,
            errors: {},
            values: {
                description: "",
                name: "",
            },
            files: [],
        };
    }

    public render() {
        return (
            <Dimmer onClick={this.exitModal}>
                <div className={Styles.modalWrapper}>
                    <FlexColumn onClick={stopPropogation} className={Styles.modal}>
                        <span className={Styles.title}>Naujas incidentas</span>
                        <span className={Styles.subTitle}>Butas {this.props.agreement.flat.name}</span>
                        <InputForm className={Styles.errors} errorsOnly={true} errors={this.state.errors.general} />

                        <InputForm
                            name="name"
                            title="Pavadinimas"
                            value={this.state.values.name}
                            errors={this.state.errors.name}
                            setValue={this.handleUpdate}
                        />

                        <InputAreaForm
                            name="description"
                            title="Aprašymas"
                            value={this.state.values.description}
                            errors={this.state.errors.description}
                            setValue={this.handleUpdate}
                            className={Styles.description}
                            maxChars={2000}
                        />

                        <ImageDropzone
                            text="Papildomi failai ir nuotraukos apie incidentą. (8 maks.)"
                            onDrop={this.handleFileUpdate}
                            maxFiles={8}
                            maxSize={5000000}
                            className={Styles.dropzone}
                        />

                        <FlexRow>
                            <Button key={1} onClick={this.createIncident} className={Styles.createButton}>
                                Sukurti
                            </Button>
                            <Button key={2} onClick={this.exitModal} className={Styles.closeButton}>
                                Uždaryti
                            </Button>
                        </FlexRow>
                    </FlexColumn>
                </div>
            </Dimmer>
        );
    }

    private handleUpdate = (name: string, value: string) =>
        this.setState((state) => ({ values: { ...state.values, [name]: value } }));

    private handleFileUpdate = (files: IPreviewFile[]) => this.setState({ files });

    private createIncident = () => {
        this.setState({ requesting: true });
        const { name, description } = this.state.values;
        IncidentService.createIncident(
            this.props.agreement.id,
            { name, description, attachments: [] },
            this.state.files
        )
            .then(this.handleResponse)
            .catch(this.handleError);
    };

    private handleResponse = (response: IApiResponse<IFaultCreateResponse>) => {
        if (response.errors !== undefined) {
            this.setState({ errors: response.errors, requesting: false });
        } else if (response.data !== undefined) {
            this.setState({ requesting: false });
            toast.success("Sėkmingai sukurtas incidentas!", {
                position: toast.POSITION.BOTTOM_CENTER,
            });
            this.props.history.push(incidentUrl(this.props.agreement.id, response.data.id));
        }
    };

    private handleError = (errors: any) => {
        console.log(errors);
        toast.error("Įvyko nežinoma klaida!", {
            position: toast.POSITION.BOTTOM_CENTER,
        });
        this.setState({ requesting: false });
    };

    private exitModal = () => {
        this.props.history.push(agreementUrl(this.props.agreement.id));
    };
}

export default CreateIncidentModal;
