import React from "react";
import FlexColumn from "../../components/FlexColumn";
import { getImageUrl } from "../../services/ApiUtilities";
import { getAddressString, IFlatListItem } from "../../services/FlatService";
import { roomOrRooms, flatUrl, joined } from "../../utilities/Utilities";
import Styles from "./FlatBox.module.css";
import SmartImg from "../../components/SmartImg";
import { withRouter, Link } from "react-router-dom";
import { RouterProps, RouteComponentProps } from "react-router";
import ContentLoader from "react-content-loader";

interface IFlatItemProps {
  flat: IFlatListItem;
}

const FlatBox = ({ flat }: IFlatItemProps) => {
  return (
    <FlexColumn className={Styles.flatBox}>
      <div className={Styles.imageWrapper}>
        <Link className={Styles.imageLink} to={flatUrl(flat.id)} />
        <SmartImg className={Styles.image} src={getImageUrl(flat.imageId)} />
      </div>
      <Link className={Styles.link} to={flatUrl(flat.id)}>
        <FlexColumn className={Styles.content}>
          <span className={Styles.address}>
            {`${flat.address.street} • ${flat.address.city} • ${flat.address.country}`}
          </span>
          <span className={Styles.details}>
            {`${flat.floor} aukštas • ${flat.area} m² • ${flat.roomCount} ${roomOrRooms(flat.roomCount)}  • ${flat.price} Eur`}
          </span>
          <span className={Styles.title}>{flat.name}</span>
        </FlexColumn>
      </Link>
    </FlexColumn>
  );
};

export const FlatBoxLoader = () => (
  <div className={Styles.flatBox}>
    <div className={Styles.loader} >
      <ContentLoader
        speed={2}
        height={334}
        width={400}
        primaryColor="#f3f3f3"
        secondaryColor="#ecebeb"
      >
        <rect x="0" y="0" rx="5" ry="5" width="400" height="266" />
        <rect x="0" y="276" rx="4" ry="4" width="240" height="12" />
        <rect x="0" y="296" rx="4" ry="4" width="290" height="12" />
        <rect x="0" y="316" rx="4" ry="4" width="400" height="18" />
      </ContentLoader>
    </div>
  </div>
);
export default FlatBox;
