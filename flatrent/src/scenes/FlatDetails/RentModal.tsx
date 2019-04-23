import React, { Component } from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { toast } from "react-toastify";
import Dimmer from "../../components/Dimmer";
import FlexRow from "../../components/FlexRow";
import FlatService, { IFlatDetails, IFlatDetailsResponse } from "../../services/FlatService";
import { flatUrl, stopPropogation } from "../../utilities/Utilities";
import Styles from "./RentModal.module.css";

import Moment from "moment";
import "moment/locale/lt";
import "react-dates/initialize";
import "react-dates/lib/css/_datepicker.css";
import {DateRangePicker, FocusedInputShape, SingleDatePicker, DateRangePickerPhrases} from "react-dates";
import FlexColumn from "../../components/FlexColumn";
import Button from "../../components/Button";
import FlexDropzone from "../../components/FlexDropzone";
import { InputAreaForm } from "../../components/InputForm";
// import moment = require("moment");
Moment.locale("lt");

interface IRentModalState {
  loading: boolean;
  errors: { [key: string]: string[] };
  values: { [key: string]: string; from: string; to: string; comments: string };
  startDate: Moment.Moment | null;
  endDate: Moment.Moment | null;
  focusedInput: FocusedInputShape | null;
}

const getOffsetDate = (years: number, months: number, days: number): string => {
  const date = new Date();
  date.setFullYear(date.getFullYear() + years);
  date.setMonth(date.getMonth() + months);
  date.setDate(date.getDate() + days);

  const dd = date.getDate();
  let ddStr = "" + dd;
  const mm = date.getMonth() + 1;
  let mmStr = "" + dd;
  const yyyy = date.getFullYear();

  if (dd < 10) {
    ddStr = "0" + dd;
  }

  if (mm < 10) {
    mmStr = "0" + mm;
  }

  return `${yyyy}-${mmStr}-${ddStr}`;
};

export interface IRentModalProps {
  flat: IFlatDetails;
}

class RentModal extends Component<RouteComponentProps<{ id: string }> & IRentModalProps, IRentModalState> {
  constructor(props: RouteComponentProps<{ id: string }> & IRentModalProps) {
    super(props);
    console.log("construct", props);
    this.state = {
      loading: true,
      errors: {},
      values: { from: getOffsetDate(0, 0, 0), to: getOffsetDate(0, 1, 7), comments: "" },
      startDate: null,
      endDate: null,
      focusedInput: null,
    };
  }

  public render() {
    const { flat } = this.props;

    return (
      <Dimmer onClick={this.exitModal}>
        <div className={Styles.modalWrapper}>
          <FlexColumn onClick={stopPropogation} className={Styles.modal}>
            <span className={Styles.title}>
              Buto nuoma
            </span>
            <FlexRow className={Styles.subRow}>
              <span className={Styles.subTitle}>
                Pageidavimai nuomininkui:
              </span>
              <span className={Styles.subText}>
                {flat.tenantRequirements}
              </span>
            </FlexRow>

            <FlexRow className={Styles.subRow}>
              <span className={Styles.subTitle}>
                Kaina mėnesiui:
              </span>
              <span className={Styles.subText}>
                {flat.price} Eur
              </span>
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
              <span className={Styles.subTitle}>
                Nuomos trukmė:
              </span>
              <span className={Styles.subText}>
                {this.getDaysSelected()} d.
              </span>
            </FlexRow>

            <InputAreaForm className={Styles.inputArea} name="description" setValue={() => {}} title="Papildoma informacija" />
            <FlexDropzone
              className={Styles.dropzone}
              onDrop={() => {}}
              text={"Pridėti papildomus failus nurodytus pageidavimuose, bei patvirtinančius Jūsų nuomos sutarties prašymą."}
            />
            <Button className={Styles.button} onClick={() => {}}>Pasirašyti</Button>
          </FlexColumn>
        </div>
      </Dimmer>
    );
  }

  private isDayBlocked = (day: Moment.Moment): boolean => {
    if (this.state.focusedInput === "startDate") {
      return day.isBefore(Moment().add(14, 'days'));
    }
    if (this.state.focusedInput === "endDate" && this.state.startDate !== null) {
      // console.log(day.diff(this.state.startDate, "month", true));
      return day.isBefore(this.state.startDate); // || day.diff(this.state.startDate, "month", true).toString().indexOf(".") !== -1;
    }
    return false;
  }

  private getDaysSelected = () =>
    this.state.startDate === null || this.state.endDate === null
      ? "0"
      : this.state.endDate.diff(this.state.startDate, "day");

  private getInfo = () =>
    <>
      <div className={Styles.infoInPicker}>Nuoma gali prasidėti už 14 d.</div>
      <div className={Styles.infoInPicker}>Trumpiausias nuomos laikotarpis: {this.props.flat.minimumRentDays} d.</div>
    </>;

  private updateDates = ({startDate, endDate}: {startDate: Moment.Moment | null, endDate: Moment.Moment | null}) =>
    this.setState({ startDate, endDate });

  private updateFocusedInput = (focusedInput: FocusedInputShape | null) =>
    this.setState({ focusedInput });

  private handleFail() {
    toast.error("Įvyko nežinoma klaida.");
  }

  private exitModal = (evt: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
    this.props.history.push(flatUrl(this.props.flat.id));
  }
}

export default RentModal;
