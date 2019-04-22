import React, { Component, ReactNode } from "react";
import { RouteComponentProps } from "react-router-dom";
import { toast } from "react-toastify";
import FlatService, { IFlatListItem, IFlatListResponse } from "../../services/FlatService";
import CreateFlatBox from "./FlatCreateBox";
import FlatItem from "./FlatItem";
import Card from "../../components/Card";
import FlatFilters from "./FlatFilters";
import FlatBox from "./FlatBox";
import FlexRow from "../../components/FlexRow";
import Styles from "./FlatList.module.css";

class FlatList extends Component<
  RouteComponentProps,
  { pageSize: number; page: number; rented: boolean; flats: IFlatListItem[] }
> {
  constructor(props: Readonly<RouteComponentProps>) {
    super(props);
    this.state = { pageSize: 20, page: 1, rented: false, flats: [] };
    this.getFlats(this.state.pageSize, this.state.page, this.state.rented);
  }

  public render() {
    return (
      <FlexRow className={Styles.flatBoxes}>
        {/* <FlatFilters onPageCountChange={console.log} onShowRentedChange={this.handleShowRentedChange} /> */}
        {/* <CreateFlatBox /> */}
        {this.getFlatItems()}
      </FlexRow>
    );
  }

  private handleShowRentedChange = (rented: boolean) => {
    this.setState({ rented });
    this.getFlats(this.state.pageSize, this.state.page, rented);
  };

  private openFlat = (id: string) => {
    this.props.history.push(`/flat/${id}`);
  };

  private getFlatItems(): ReactNode[] {
    const flats = this.state.flats.map((flat) => <FlatBox onClick={this.openFlat} key={flat.id} flat={flat} />);
    if (flats.length > 0) {
      return flats;
    } else {
      return [<Card key={1}>Nuomojamų butų nėra</Card>];
    }
  }

  private getFlats(pageSize: number, page: number, rented: boolean) {
    FlatService.getFlats(pageSize, pageSize * (page - 1), rented)
      .then(this.handleFlatResult)
      .catch(this.handleFail);
  }

  private handleFlatResult = (result: IFlatListResponse) => {
    if (result.errors !== undefined) {
      const errors = Object.keys(result.errors).map((key) => result.errors![key].join("\n"));
      errors.forEach((error) => toast.error(error));
    } else if (result.flats !== undefined) {
      this.setState({ flats: result.flats });
    }
  };

  private handleFail(e: any) {
    toast.error("Įvyko nežinoma klaida.");
  }
}

export default FlatList;
