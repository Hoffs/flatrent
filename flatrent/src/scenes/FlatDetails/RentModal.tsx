import React, { Component } from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { toast } from "react-toastify";
import FlexRow from "../../components/FlexRow";
import FlatService, { IFlatDetails, IFlatDetailsResponse } from "../../services/FlatService";
import Styles from "./FlatDetails.module.css";
import Dimmer from "../../components/Dimmer";
import { flatUrl, stopPropogation } from "../../utilities/Utilities";

import {DateRangePicker} from "react-dates";
import "react-dates/initialize";
import "react-dates/lib/css/_datepicker.css";
import FlexColumn from "../../components/FlexColumn";

interface IRentModalState {
  loading: boolean;
  flat?: IFlatDetails;
  errors: { [key: string]: string[] };
  values: { [key: string]: string; from: string; to: string; comments: string };
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

class RentModal extends Component<RouteComponentProps<{ id: string }>, IRentModalState> {
  constructor(props: RouteComponentProps<{ id: string }>) {
    super(props);
    this.state = {
      loading: true,
      flat: undefined,
      errors: {},
      values: { from: getOffsetDate(0, 0, 0), to: getOffsetDate(0, 1, 7), comments: "" },
    };
  }

  public render() {
    const { flat } = this.state;

    return (
      <Dimmer onClick={this.exitModal}>
        <div className={Styles.modalWrapper}>
          <FlexColumn onClick={stopPropogation} className={Styles.modal}>
            <DateRangePicker
              startDate={null} // momentPropTypes.momentObj or null,
              startDateId="asdasd" // PropTypes.string.isRequired,
              endDate={null} // momentPropTypes.momentObj or null,
              endDateId="afasd" // PropTypes.string.isRequired,
              onDatesChange={({ startDate, endDate }) => console.log(startDate, endDate)} // PropTypes.func.isRequired,
              focusedInput={"startDate"} // PropTypes.oneOf([START_DATE, END_DATE]) or null,
              onFocusChange={focusedInput => console.log(focusedInput)} // PropTypes.func.isRequired,
            />
          </FlexColumn>
        </div>
      </Dimmer>
    );
  }

  private handleFail() {
    toast.error("Įvyko nežinoma klaida.");
  }

  private exitModal = (evt: React.MouseEvent<HTMLDivElement, MouseEvent>) => {
    this.props.history.push(flatUrl(this.props.match.params.id));
  }
}

export default RentModal;
