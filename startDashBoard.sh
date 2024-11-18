#!/bin/bash

host=$(ifconfig | grep -E "([0-9]{1,3}\.){3}[0-9]{1,3}" | grep -v 127.0.0.1 | awk '{ print $2 }' | head -n1)

export API_URL=${host}

#export DOCKERHOST=${host}

program=$0
function usage {
    echo ""
    echo "Starts dashboard for PA monitoring"
    echo ""
    echo "usage: $program --MOCK_CLIENT boolean"
    echo ""
    echo "  --MOCK_CLIENT boolean   enable/disable mock client"
    echo ""
}


while [ $# -gt 0 ]; do
    if [[ $1 == "--help" ]]; then
        usage
        exit 0
    elif [[ $1 == "--"* ]]; then
        v="${1/--/}"
        declare "$v"="$2"
        shift
    fi
    shift
done

if $MOCK_CLIENT ; then
  export MOCK_CLIENT="true"
else
  export MOCK_CLIENT="false"
fi

docker-compose up -d