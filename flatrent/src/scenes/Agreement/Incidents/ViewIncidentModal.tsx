import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Styles from "./ViewIncidentModal.module.css";

import Moment from "moment";
// tslint:disable-next-line: no-submodule-imports
import "moment/locale/lt";
// tslint:disable-next-line: no-submodule-imports
import "react-dates/initialize";
// tslint:disable-next-line: no-submodule-imports
import "react-dates/lib/css/_datepicker.css";
import { CompactAttachmentPreview } from "../../../components/AttachmentPreview";
import Button from "../../../components/Button";
import ConversationBox from "../../../components/ConversationBox";
import Dimmer from "../../../components/Dimmer";
import FlexColumn from "../../../components/FlexColumn";
import FlexRow from "../../../components/FlexRow";
import { InputForm, NumberInputForm } from "../../../components/InputForm";
import { TextRowLoader } from "../../../components/Loaders";
import IncidentService from "../../../services/IncidentService";
import { IAgreementDetails } from "../../../services/interfaces/AgreementInterfaces";
import { IApiResponse, IErrorResponse, IInputValues } from "../../../services/interfaces/Common";
import { IIncidentCreateResponse, IIncidentDetails, IShortIncidentDetails } from "../../../services/interfaces/IncidentInterfaces";
import UserService from "../../../services/UserService";
import { agreementUrl, stopPropogation } from "../../../utilities/Utilities";

Moment.locale("lt");

export interface IViewIncidentModalProps {
    agreement: IAgreementDetails;
    incidents: IShortIncidentDetails[];
}

interface IViewIncidentModalState {
    incident?: IIncidentDetails;
    requesting: boolean;
    values: IInputValues;
    errors: IErrorResponse;
}

class ViewIncidentModal extends Component<
    RouteComponentProps<{ id: string; incidentId: string }> & IViewIncidentModalProps,
    IViewIncidentModalState
> {
    constructor(props: RouteComponentProps<{ id: string; incidentId: string }> & IViewIncidentModalProps) {
        super(props);
        this.state = {
            requesting: false,
            errors: {},
            values: {
                price: "0",
            },
        };
        this.fetchIncident(props.match.params.incidentId);
    }

    public render() {
        let content;
        const { incident } = this.state;
        if (incident === undefined) {
            content = (
                <>
                    <TextRowLoader rows={8} width={600} />
                </>
            );
        } else {
            const buttons = [];
            const fixInput = [];
            if (UserService.userId() === incident.owner.id && !incident.repaired) {
                buttons.push(
                    <Button key={1} onClick={this.fixIncident} className={Styles.fixButton}>
                        Pažymėti sutaisytu
                    </Button>
                );
                fixInput.push(
                    <InputForm className={Styles.errors} errorsOnly={true} errors={this.state.errors.general} />,
                    <NumberInputForm
                        name="price"
                        title="Taisymo kaina (Eur)"
                        value={this.state.values.price}
                        errors={this.state.errors.price}
                        setValue={this.handleUpdate}
                        minValue={0}
                    />
                );
            } else if (UserService.userId() === incident.tenant.id && !incident.repaired) {
                buttons.push(
                    <Button key={1} onClick={this.deleteIncident} className={Styles.deleteButton}>
                        Ištrinti
                    </Button>
                );
            }
            buttons.push(
                <Button key={2} onClick={this.exitModal} className={Styles.closeButton}>
                    Uždaryti
                </Button>
            );
            content = (
                <>
                    <span className={Styles.title}>Incidentas</span>
                    <span className={Styles.subTitle}>{incident.name}</span>

                    <span className={Styles.status}>Statusas: {this.getIsRepaired(incident)}</span>
                    <span className={Styles.price}>Kaina: {this.getPrice(incident)}</span>

                    <span className={Styles.sectionTitle}>Aprašymas</span>
                    <span className={Styles.description}>{incident.description}</span>

                    <span className={Styles.sectionTitle}>Pridėti failai</span>
                    <CompactAttachmentPreview className={Styles.preview} attachments={incident.attachments} />

                    {fixInput}
                    <FlexRow>{buttons}</FlexRow>

                    <ConversationBox className={Styles.conversation} conversation={incident.conversation} />
                </>
            );
        }

        return (
            <Dimmer onClick={this.exitModal}>
                <div className={Styles.modalWrapper}>
                    <FlexColumn onClick={stopPropogation} className={Styles.modal}>
                        {content}
                    </FlexColumn>
                </div>
            </Dimmer>
        );
    }

    private getIsRepaired = (incident: IShortIncidentDetails) => (incident.repaired ? "Sutaisytas" : "Nesutaisytas");
    private getPrice = (incident: IShortIncidentDetails) => (incident.price === 0 ? "Nenustatyta" : incident.price);

    private handleUpdate = (name: string, value: string) =>
        this.setState((state) => ({ values: { ...state.values, [name]: value } }));

    private fetchIncident = async (incidentId: string) => {
        this.setState({ requesting: true });
        try {
            const response = await IncidentService.getIncident(this.props.agreement.id, incidentId);
            if (response.errors !== undefined) {
                toast.error("Įvyko nežinoma klaida!", {
                    position: toast.POSITION.BOTTOM_CENTER,
                });
                this.exitModal();
            } else if (response.data !== undefined) {
                this.setState({ incident: response.data });
            }
        } catch (error) {
            console.log(error);
            this.exitModal();
        }
    };

    private deleteIncident = async () => {
        const response = await IncidentService.deleteIncident(
            this.props.agreement.id,
            this.props.match.params.incidentId
        );
        if (response.errors !== undefined) {
            this.setState({ errors: response.errors, requesting: false });
        } else {
            toast.success("Sėkmingai ištrintas incidentas!", {
                position: toast.POSITION.BOTTOM_CENTER,
            });
            const idx = this.props.incidents.findIndex((i) => i.id === this.props.match.params.incidentId);
            if (idx !== -1) {
                delete this.props.incidents[idx];
            }
            this.exitModal();
        }
    };

    private fixIncident = () => {
        this.setState({ requesting: true });
        const { price } = this.state.values;
        IncidentService.updateIncident(
            this.props.agreement.id,
            this.props.match.params.incidentId,
            Number.parseFloat(price)
        )
            .then(this.handleResponse)
            .catch(this.handleError);
    };

    private handleResponse = (response: IApiResponse<IIncidentCreateResponse>) => {
        if (response.errors !== undefined) {
            this.setState({ errors: response.errors, requesting: false });
        } else if (response.data !== undefined) {
            this.setState({ requesting: false });
            toast.success("Sėkmingai incidentas pažymėtas sutaisytu!", {
                position: toast.POSITION.BOTTOM_CENTER,
            });
            const repairedIncident = this.props.incidents.find((i) => i.id === this.props.match.params.incidentId);
            const { price } = this.state.values;
            if (repairedIncident !== undefined) {
                repairedIncident.price = Number.parseFloat(price);
                repairedIncident.repaired = true;
            }
            this.exitModal();
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

export default ViewIncidentModal;
