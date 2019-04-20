import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader"
import Button from "../../components/Button";
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import { TextRowLoader } from "../../components/Loaders";
import { getAddressString, IFlatDetails } from "../../services/FlatService";
import { dayOrDays, joined, roomOrRooms, flatRentUrl } from "../../utilities/Utilities";
import Styles from "./FlatDetails.module.css";
import { withRouter, RouteComponentProps } from "react-router-dom";

const RentPanel = ({history, flat}: {flat?: IFlatDetails} & RouteComponentProps) => {
  if (flat === undefined) {
    return (
      <FlexColumn className={Styles.panel}>
        <TextRowLoader rows={12} width={200} />
      </FlexColumn>
    );
  }

  const rentPeriod = (days: number): ReactNode => (<>Trumpiausias nuomos laikotarpis {days} {dayOrDays(days)}.</>);
  const goToRent = () => history.push(flatRentUrl(flat.id));

  return (
    <FlexColumn className={Styles.panel}>
      <span className={Styles.sectionTitle}>Buto nuoma</span>
      <span className={Styles.isRented}>Šiuo metu butas yra {flat.isRented ? "išnuomotas" : "laisvas"}.</span>

      <FlexRow className={Styles.minimumRentDays}>
        <span className={Styles.leftHand}>Trumpiausias nuomos laikotarpis:</span>
        <span className={Styles.rightHand}>{flat.minimumRentDays} d.</span>
      </FlexRow>

      <FlexRow className={Styles.rentPriceRow}>
        <span className={joined(Styles.rentPriceText, Styles.leftHand)}>Nuomos kaina mėnesiui:</span>
        <span className={joined(Styles.rentPrice, Styles.rightHand)}>{flat.price} Eur</span>
      </FlexRow>

      <Button onClick={goToRent}>Nuomotis</Button>
    </FlexColumn>
  );
};

export default withRouter(RentPanel);
