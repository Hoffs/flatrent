import React, { Component, ReactNode } from "react";
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
import ImageCarousel from "../../components/ImageCarousel";
import InputArea from "../../components/InputArea";
import { materialTheme } from "../../Mui/CustomMuiTheme.js";
import UserService, { Policies } from "../../services/UserService";
import { joinClasses as joined, dayOrDays } from "../../utilities/Utilities";
import { getAvatarUrl } from "../../services/ApiUtilities";

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

const roomOrRooms = (roomCount: number): string => roomCount > 1 ? "kambariai" : "kambarys";

class FlatDetails extends Component<RouteComponentProps<{ id: string }>, IFlatDetailsState> {
  constructor(props: RouteComponentProps<{ id: string }>) {
    super(props);
    this.state = {
      requesting: false,
      flat: undefined,
      showRentCard: false,
      errors: {},
      values: { from: getOffsetDate(0, 0, 0), to: getOffsetDate(0, 1, 7), comments: "" },
    };
    this.fetchFlat(props.match.params.id);
  }

  public render() {
    const { flat } = this.state;
    if (flat === undefined) {
      return <Card className={Styles.loadingCard}>Kraunasi...</Card>;
    }

    const featureNodes = this.getFeatures(flat);

    return (
      <>
        <ImageCarousel
          wrapperClassName={Styles.imageWrapper}
          imageIds={flat.images.map((fi) => fi.id)}
        />
        <div className={Styles.detailsContainer}>
          <FlexRow className={Styles.sectionEnd}>
            <FlexColumn className={Styles.titleInfo}>
              <span className={Styles.flatTitle}>{flat.name}</span>
              <span className={Styles.subTitle}>{getAddressString(flat.address)}</span>

              <span className={Styles.sectionTitle}>Nuomojamas butas</span>
              <FlexRow className={Styles.flatCharacteristics}>
                <span>{flat.isFurnished ? "Įrengtas" : "Neįrengtas"}</span>
                <span>{flat.floor} aukštas</span>
                <span>{flat.roomCount} {roomOrRooms(flat.roomCount)}</span>
                <span>{flat.area} kv.m.</span>
                <span>{flat.yearOfConstruction} metai</span>
              </FlexRow>

              <span className={Styles.sectionTitle}>Ypatybės</span>
              <FlexRow className={Styles.features}>
                {featureNodes}
              </FlexRow>

            </FlexColumn>
            <FlexColumn className={Styles.userInfo}>
              <span className={Styles.avatarName}>{flat.owner.firstName}</span>
              <div className={Styles.avatarWrapper}><img className={Styles.avatar} src={getAvatarUrl(flat.owner.id)} /></div>
              <span className={Styles.contactOwner}>Susisiekti</span>
            </FlexColumn>
          </FlexRow>

          {/* <FlexColumn className={Styles.sectionEnd}> */}

            {/* <span className={Styles.ownerInfoTitle}>Nuomotojas:</span>
            <FlexRow className={Styles.ownerInfo}>
              <span>tel.nr. {flat.owner.phoneNumber}</span>
              <span>el. paštas <a href={`mailto:${flat.owner.email}`}>{flat.owner.email}</a></span>
            </FlexRow> */}

          {/* </FlexColumn> */}

          <span className={Styles.sectionTitle}>Aprašymas</span>
          <span className={joined(Styles.flatDescription, Styles.sectionEnd)}>
            {flat.description}
          </span>

          <span className={Styles.sectionTitle}>Pageidavimai nuomininkui</span>
          <span className={Styles.tenantRequirements}>
            {flat.tenantRequirements}
          </span>
          <span className={joined(Styles.minimumRentDays, Styles.sectionEnd)}>
            Trumpiausias nuomos laikotarpis {flat.minimumRentDays} {dayOrDays(flat.minimumRentDays)}.
          </span>

          <FlexRow className={Styles.titleCardContent}>
            {/* {this.getTitleButton()} */}
          </FlexRow>
        </div>
      </>
    );
  }

  private getTitleButton = () => {
    return !UserService.hasRoles(...Policies.User) ? (
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

  private getFeatures = (flat: IFlatDetails): ReactNode[] => {
    const { features } = flat;
    if (features.length === 0) { features.push("Nėra"); }
    return flat.features.map((feature) => (
      <span className={Styles.feature}>{feature}</span>
    ));
  }

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
