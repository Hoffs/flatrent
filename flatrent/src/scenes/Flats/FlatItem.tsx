import React from "react";
import Button from "../../components/Button";
import Card from "../../components/Card";
import { getAddressString, IFlatListItem } from "../../services/FlatService";
import Styles from "./FlatItem.module.css";
import { ApiHostname } from "../../services/Settings";

interface IFlatItemProps {
  flat: IFlatListItem;
  onClick: (id: string) => void;
}

const FlatItem = ({ flat, onClick }: IFlatItemProps) => {
  const func = () => onClick(flat.id);
  return (
    <Card className={Styles.flat}>
      <div className={Styles.image}>
        <img src={`${ApiHostname}api/image/${flat.id}`} />
      </div>
      <div className={Styles.content}>
        <div className={Styles.text}>
          <span className={Styles.title}>{flat.name}</span>
          <span className={Styles.address}>{getAddressString(flat.address)}</span>
        </div>
        <Button onClick={func} className={Styles.openButton}>
          Plačiau
        </Button>
      </div>
    </Card>
  );
};

export default FlatItem;
