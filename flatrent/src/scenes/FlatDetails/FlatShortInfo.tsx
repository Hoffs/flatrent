import React, { Component, ReactNode } from "react";
import ContentLoader from "react-content-loader"
import FlexColumn from "../../components/FlexColumn";
import FlexRow from "../../components/FlexRow";
import { getAddressString } from "../../services/FlatService";
import { roomOrRooms } from "../../utilities/Utilities";
import Styles from "./FlatDetails.module.css";
import { TextRowLoader } from "../../components/Loaders";
import { IFlatDetails } from "../../services/interfaces/FlatServiceInterfaces";

const getFeatures = (flat?: IFlatDetails): ReactNode[] => {
  const features = [];
  if (flat === undefined || flat.features.length === 0) {
    features.push("Nėra"); } else { features.push(...flat.features); }

  return features.map((feature, idx) => (
    <span key={idx} className={Styles.feature}>{feature}</span>
  ));
};

const FlatShortInfo = ({flat}: {flat?: IFlatDetails}) => {
  if (flat === undefined) {
    return (
      <FlexColumn>
        <TextRowLoader rows={3} width={500} />
        <br />
        <br />
        <TextRowLoader rows={9} width={500} />
      </FlexColumn>
    )
  }

  return (
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
        {getFeatures(flat)}
      </FlexRow>

    </FlexColumn>
  );
};

export default FlatShortInfo;
