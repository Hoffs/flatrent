import React, { Component } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import Dimmer from "../../../../components/Dimmer";
import FlexRow from "../../../../components/FlexRow";
import FlatService from "../../../../services/FlatService";
import { flatUrl, stopPropogation } from "../../../../utilities/Utilities";
import Styles from "./RentModal.module.css";

import Moment from "moment";
// tslint:disable-next-line: no-submodule-imports
import "moment/locale/lt";
import { DateRangePicker, FocusedInputShape } from "react-dates";
// tslint:disable-next-line: no-submodule-imports
import "react-dates/initialize";
// tslint:disable-next-line: no-submodule-imports
import "react-dates/lib/css/_datepicker.css";
import Button from "../../../../components/Button";
import { ImageDropzone } from "../../../../components/Dropzones";
import { IPreviewFile } from "../../../../components/Dropzones/ImageDropzone";
import FlexColumn from "../../../../components/FlexColumn";
import { InputAreaForm, InputForm } from "../../../../components/InputForm";
import { IApiResponse } from "../../../../services/interfaces/Common";
import { IAgreementCreateResponse, IFlatDetails } from "../../../../services/interfaces/FlatServiceInterfaces";

Moment.locale("lt");

export interface IRentModalProps {
    flat: IFlatDetails;
}

interface IRentModalState {
    requesting: boolean;
    errors: { [key: string]: string[] };
    startDate: Moment.Moment | null;
    endDate: Moment.Moment | null;
    focusedInput: FocusedInputShape | null;
    comments: string;
    files: IPreviewFile[];
}

class RentModal extends Component<RouteComponentProps<{ id: string }> & IRentModalProps, IRentModalState> {
    constructor(props: RouteComponentProps<{ id: string }> & IRentModalProps) {
        super(props);
        console.log("construct", props);
        this.state = {
            comments: "",
            endDate: null,
            errors: {},
            files: [],
            focusedInput: null,
            requesting: false,
            startDate: null,
        };
    }

    public render() {
        const { flat } = this.props;
        const { errors } = this.state;

        return (
            <Dimmer onClick={this.exitModal}>
                <div className={Styles.modalWrapper}>
                    <FlexColumn onClick={stopPropogation} className={Styles.modal}>
                        <span className={Styles.title}>Buto nuoma</span>
                        <InputForm className={Styles.generalErrors} errorsOnly={true} errors={errors.general} />
                        <InputForm className={Styles.generalErrors} errorsOnly={true} errors={errors.from} />
                        <InputForm className={Styles.generalErrors} errorsOnly={true} errors={errors.to} />
                        <FlexRow className={Styles.subRow}>
                            <span className={Styles.subTitle}>Pageidavimai nuomininkui:</span>
                            <span className={Styles.subText}>{flat.tenantRequirements}</span>
                        </FlexRow>

                        <FlexRow className={Styles.subRow}>
                            <span className={Styles.subTitle}>Kaina mėnesiui:</span>
                            <span className={Styles.subText}>{flat.price} Eur</span>
                        </FlexRow>

                        <div className={Styles.datepickerWrapper}>
                            <DateRangePicker
                                minimumNights={flat.minimumRentDays}
                                startDate={this.state.startDate}
                                startDateId="startDate"
                                endDate={this.state.endDate}
                                endDateId="endDate"
                                onDatesChange={this.updateDates}
                                focusedInput={this.state.focusedInput}
                                onFocusChange={this.updateFocusedInput}
                                numberOfMonths={1}
                                anchorDirection={"left"}
                                hideKeyboardShortcutsPanel={true}
                                displayFormat={"YYYY-MM-DD"}
                                startDatePlaceholderText={"Nuo"}
                                endDatePlaceholderText={"Iki"}
                                renderCalendarInfo={this.getInfo}
                                isDayBlocked={this.isDayBlocked}
                                disabled={this.state.startDate === null ? "endDate" : false}
                            />
                        </div>

                        <FlexRow className={Styles.subRow}>
                            <span className={Styles.subTitle}>Nuomos trukmė:</span>
                            <span className={Styles.subText}>{this.getDaysSelected()} d.</span>
                        </FlexRow>

                        <InputAreaForm
                            errors={errors.comments}
                            className={Styles.inputArea}
                            name="comments"
                            value={this.state.comments}
                            setValue={this.updateComments}
                            title="Papildoma informacija"
                            maxChars={2000}
                        />
                        <ImageDropzone
                            className={Styles.dropzone}
                            onDrop={this.updateFiles}
                            text="Pridėti papildomus failus nurodytus pageidavimuose,
              bei papildančius Jūsų nuomos sutarties prašymą.
              (8 maks.)"
                            maxSize={5000000}
                            maxFiles={8}
                        />
                        <FlexRow>
                            <Button
                                disabled={this.isDisabled()}
                                className={Styles.button}
                                onClick={this.submitRentRequest}
                            >
                                Pasirašyti
                            </Button>
                            <Button className={Styles.button} onClick={this.exitModal}>
                                Atšaukti
                            </Button>
                        </FlexRow>
                    </FlexColumn>
                </div>
            </Dimmer>
        );
    }

    private isDisabled = () => this.state.requesting || this.state.startDate === null || this.state.endDate === null;

    private submitRentRequest = () => {
        this.setState({ requesting: true });
        const { startDate, endDate, comments, files } = this.state;
        FlatService.rentFlat(
            this.props.flat.id,
            {
                attachments: files.map((f) => ({ name: f.name })),
                comments,
                from: startDate!.toISOString(),
                to: endDate!.toISOString(),
            },
            files
        )
            .then(this.handleResponse)
            .catch(this.handleError);
    };

    private handleResponse = (response: IApiResponse<IAgreementCreateResponse>) => {
        if (response.errors !== undefined) {
            this.setState({ errors: response.errors, requesting: false });
        } else {
            this.setState({ requesting: false });
            toast.success("Sėkmingai sukurtas įrašas!", {
                position: toast.POSITION.BOTTOM_CENTER,
            });
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

    private isDayBlocked = (day: Moment.Moment): boolean => {
        if (this.state.focusedInput === "startDate") {
            return day.isBefore(Moment().add(3, "days"));
        }
        if (this.state.focusedInput === "endDate" && this.state.startDate !== null) {
            return day.isBefore(this.state.startDate);
            // || day.diff(this.state.startDate, "month", true).toString().indexOf(".") !== -1;
        }
        return false;
    };

    private getDaysSelected = () =>
        this.state.startDate === null || this.state.endDate === null
            ? "0"
            : this.state.endDate.diff(this.state.startDate, "day");

    private getInfo = () => (
        <>
            <div className={Styles.infoInPicker}>Nuoma gali prasidėti už 3 d.</div>
            <div className={Styles.infoInPicker}>
                Trumpiausias nuomos laikotarpis: {this.props.flat.minimumRentDays} d.
            </div>
        </>
    );

    private updateDates = ({
        startDate,
        endDate,
    }: {
        startDate: Moment.Moment | null;
        endDate: Moment.Moment | null;
    }) => this.setState({ startDate, endDate });

    private updateFocusedInput = (focusedInput: FocusedInputShape | null) => this.setState({ focusedInput });

    private updateFiles = (files: IPreviewFile[]) => this.setState({ files });
    private updateComments = (name: string, value: string) => this.setState({ comments: value });

    private exitModal = () => {
        this.props.history.push(flatUrl(this.props.flat.id));
    };
}

export default RentModal;
