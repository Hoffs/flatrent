import React from "react";
import FlexColumn from "../../components/FlexColumn";
import { getImageUrl } from "../../services/ApiUtilities";
import { getAddressString, IFlatListItem } from "../../services/FlatService";
import { roomOrRooms, flatUrl } from "../../utilities/Utilities";
import Styles from "./FlatBox.module.css";
import SmartImg from "../../components/SmartImg";
import { withRouter, Link } from "react-router-dom";
import { RouterProps, RouteComponentProps } from "react-router";

interface IFlatItemProps {
  flat: IFlatListItem;
}

const FlatBox = ({ flat, history }: IFlatItemProps & RouteComponentProps) => {
  return (
    <FlexColumn className={Styles.flatBox}>
      <div className={Styles.imageWrapper}>
        <Link className={Styles.imageLink} to={flatUrl(flat.id)} />
        <SmartImg className={Styles.image} src={getImageUrl(flat.imageId)} />
      </div>
      <FlexColumn className={Styles.content}>
        <Link to={flatUrl(flat.id)}>
          <span className={Styles.address}>
            {`${flat.address.street} • ${flat.address.city} • ${flat.address.country}`}
          </span>
          <span className={Styles.title}>{flat.name}</span>
          <span className={Styles.details}>
            {`${flat.floor} aukštas • ${flat.area} m² • ${flat.roomCount} ${roomOrRooms(flat.roomCount)}  • ${flat.price} Eur`}
          </span>
        </Link>
      </FlexColumn>
    </FlexColumn>
  );
};

export default withRouter(FlatBox);