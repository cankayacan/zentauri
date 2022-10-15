import { h } from "preact";
import { useEffect, useState } from "preact/hooks";
import { Style } from "preact/jsx";

interface Props {
  radius: number;
  thickness: number;
}

const DURATION_IN_SECONDS = 1.5;

export const Spinner = ({ radius, thickness }: Props) => {
  const loader: Style = {
    borderWidth: thickness,
    borderColor: "#EAF0F6",
    borderRadius: 50,
    borderTopWidth: thickness,
    borderTopColor: "#4E7CFF",
    width: radius,
    height: radius,
    rotate: 0,
    transitionDuration: [DURATION_IN_SECONDS],
    transitionTimingFunction: ["EaseOutBack"],
  };

  const loaderTurning: Style = {
    ...loader,
    rotate: 360,
  };

  const [turned, setTurned] = useState(false);

  // for the initial spin
  useEffect(() => setTurned(true), []);

  useEffect(() => {
    const timeout = setTimeout(() => {
      setTurned(!turned);
    }, DURATION_IN_SECONDS * 1000);

    return () => {
      clearTimeout(timeout);
    };
  }, [turned]);

  return <div style={turned ? loaderTurning : loader} />;
};
