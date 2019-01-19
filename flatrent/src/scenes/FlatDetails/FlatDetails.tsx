import React, { Component } from "react";
import { RouteComponentProps, withRouter } from "react-router-dom";
import { toast } from "react-toastify";
import Button from "../../components/Button";
import Card from "../../components/Card";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import InputForm from "../../components/InputForm";
import FlatService, { getAddressString, IFlatDetails, IFlatDetailsResponse } from "../../services/FlatService";
import { ApiHostname } from "../../services/Settings";
import Styles from "./FlatDetails.module.css";

import DateFnsUtils from "@date-io/date-fns";
import { MuiThemeProvider } from "@material-ui/core";
import { format } from "date-fns";
import locale from "date-fns/locale/lt";
import { InlineDatePicker, MuiPickersUtilsProvider } from "material-ui-pickers";
import InputArea from "../../components/InputArea";
import { materialTheme } from "../../Mui/CustomMuiTheme.js";
import UserService, { Policies } from "../../services/UserService";

interface IFlatDetailsState {
  flat?: IFlatDetails;
  showRentCard: boolean;
  errors: { [key: string]: string[] };
  values: { [key: string]: string; from: string; to: string; comments: string };
  requesting: boolean;
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

const doNothing = () => {};

class FlatDetails extends Component<RouteComponentProps<{ id: string }>, IFlatDetailsState> {
  constructor(props: RouteComponentProps<{ id: string }>) {
    super(props);
    this.state = {
      requesting: false,
      flat: undefined,
      showRentCard: false,
      errors: {},
      values: { from: getOffsetDate(0, 0, 7), to: getOffsetDate(0, 1, 7), comments: "" },
    };
    this.fetchFlat(props.match.params.id);
  }

  public render() {
    if (this.state.flat === undefined) {
      return <Card className={Styles.loadingCard}>Kraunasi...</Card>;
    }

    const { flat } = this.state;

    return (
      <>
        <Card key="titleCard" className={Styles.titleCard}>
          <div className={Styles.image}>
            <img src={`${ApiHostname}api/image/${flat.id}`} />
          </div>
          <FlexRow className={Styles.titleCardContent}>
            <FlexColumn>
              <FlexRow className={Styles.topTitleRow}>
                <span className={Styles.title}>{flat.name}</span>
                <span className={Styles.year}>Statybos metai: {flat.yearOfConstruction}</span>
              </FlexRow>
              <span className={Styles.subTitle}>{getAddressString(flat.address)}</span>
            </FlexColumn>
            {this.getTitleButton()}
          </FlexRow>
        </Card>

        {this.getDetailsCard(flat)}
        {this.getRentCard(flat)}
      </>
    );
  }

  private getTitleButton = () => {
    return !UserService.hasRoles(...Policies.Client) ? (
      <></>
    ) : (
      <Button onClick={this.handleTitleButton} outline={true}>
        {this.state.showRentCard ? "Aprašymas" : "Nuomotis"}
      </Button>
    );
  };

  private fetchFlat = (id: string) => {
    FlatService.getFlat(id)
      .then(this.handleResult)
      .catch(this.handleFail);
  };

  private handleTitleButton = () => {
    this.setState((state) => ({ showRentCard: !state.showRentCard }));
  };

  private getRentCard = (flat: IFlatDetails) => {
    const today = new Date();
    const fromExtraProps = {
      max: getOffsetDate(1, 0, 7),
      min: getOffsetDate(0, 0, 7),
    };
    const toExtraProps = {
      max: getOffsetDate(1, 0, 7),
      min: getOffsetDate(0, 1, 7),
    };

    if (this.state.values.from !== undefined && this.state.values.from.length > 0) {
      const split = this.state.values.from.split("-");
      const date = new Date();
      toExtraProps.min = getOffsetDate(
        Number(split[0]) - date.getFullYear(),
        Number(split[1]) - date.getMonth(),
        Number(split[2]) - date.getDate()
      );
    }

    return !this.state.showRentCard ? (
      <></>
    ) : (
      <Card key="rentCard" className={Styles.rentCard}>
        <span className={Styles.title}>Nuomos sutartis</span>
        <FlexRow className={Styles.shortRow}>
          <span className={Styles.name}>Kaina per mėnesį:</span>
          <span className={Styles.value}>{flat.price} Eur</span>
        </FlexRow>
        <InputForm errors={this.state.errors.General} errorsOnly={true} name="" title="" setValue={doNothing} />
        <FlexRow className={Styles.pickerRow}>
          <MuiThemeProvider theme={materialTheme}>
            <MuiPickersUtilsProvider utils={DateFnsUtils} locale={locale}>
              <FlexColumn>
                <InlineDatePicker
                  label="Nuo"
                  disablePast={true}
                  minDate={fromExtraProps.min}
                  minDateMessage="Per trumpas laikotarpis"
                  maxDate={fromExtraProps.max}
                  maxDateMessage="Per ilgas laikotarpis"
                  format="yyyy-MM-dd"
                  value={this.state.values.from}
                  onChange={this.handleDateFromChange}
                  variant="outlined"
                />
                <InputForm errorsOnly={true} errors={this.state.errors.From} name="" title="" setValue={doNothing} />
              </FlexColumn>
              <FlexColumn>
                <InlineDatePicker
                  label="Iki"
                  format="yyyy-MM-dd"
                  disablePast={true}
                  minDate={toExtraProps.min}
                  minDateMessage="Per trumpas laikotarpis"
                  maxDate={toExtraProps.max}
                  maxDateMessage="Per ilgas laikotarpis"
                  value={this.state.values.to}
                  onChange={this.handleDateToChange}
                  variant="outlined"
                />
                <InputForm errorsOnly={true} errors={this.state.errors.To} name="" title="" setValue={doNothing} />
              </FlexColumn>
            </MuiPickersUtilsProvider>
          </MuiThemeProvider>
        </FlexRow>
        <FlexRow>
          <InputArea
            className={Styles.textArea}
            errors={this.state.errors.from}
            name="comments"
            title="Komentarai"
            setValue={this.handleDataUpdate}
          />
        </FlexRow>
        <FlexRow className={Styles.buttonRow}>
          <Button outline={true} onClick={this.handleTitleButton} className={Styles.buttonCancel}>
            Atšaukti
          </Button>
          <Button disabled={this.state.requesting} outline={true} onClick={this.handleSubmit}>
            Pateikti
          </Button>
        </FlexRow>
      </Card>
    );
  };

  private handleSubmit = async () => {
    console.log("submit");
    try {
      this.setState({ requesting: true });
      const response = await FlatService.rentFlat(
        this.props.match.params.id,
        this.state.values.from,
        this.state.values.to
      );
      if (response.errors === undefined) {
        toast.success("Sėkmingai pateikta nuomos sutartis!", {
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

  private getDetailsCard = (flat: IFlatDetails) => {
    return this.state.showRentCard ? (
      <></>
    ) : (
      <Card key="detailsCard" className={Styles.detailsCard}>
        <span className={Styles.detailsTitle}>Buto aprašymas</span>
        <FlexColumn className={Styles.dataRow}>
          <FlexColumn>
            <span className={Styles.value}>
              {flat.owner.name}, {flat.owner.phoneNumber}, {flat.owner.email}
            </span>
            <span className={Styles.nameCol}>Savininkas</span>
          </FlexColumn>
          <FlexRow className={Styles.shortRow}>
            <span className={Styles.name}>Aukštas:</span>
            <span className={Styles.value}>{flat.floor}</span>
          </FlexRow>
          <FlexRow className={Styles.shortRow}>
            <span className={Styles.name}>Kambarių skaičius:</span>
            <span className={Styles.value}>{flat.roomCount}</span>
          </FlexRow>
          <FlexRow className={Styles.shortRow}>
            <span className={Styles.name}>Plotas:</span>
            <span className={Styles.value}>{flat.area} kv. m.</span>
          </FlexRow>
          <FlexRow className={Styles.shortRow}>
            <span className={Styles.name}>Kaina:</span>
            <span className={Styles.value}>{flat.price} Eur</span>
          </FlexRow>
        </FlexColumn>

        <span className={Styles.detailsText}>{flat.description}</span>
      </Card>
    );
  };

  private handleDataUpdate = (key: string, value: string) =>
    this.setState((state) => ({ values: { ...state.values, [key]: value } }));
  private handleDateFromChange = (date: Date) =>
    this.setState((state) => ({ values: { ...state.values, from: format(date, "yyyy-MM-dd") } }));
  private handleDateToChange = (date: Date) =>
    this.setState((state) => ({ values: { ...state.values, to: format(date, "yyyy-MM-dd") } }));

  private handleResult = (result: IFlatDetailsResponse) => {
    if (result.errors !== undefined) {
      const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
    } else if (result.flat !== undefined) {
      this.setState({ flat: result.flat });
    }
  };

  private handleFail(e: any) {
    toast.error("Įvyko nežinoma klaida.");
  }
}

export default withRouter(FlatDetails);
