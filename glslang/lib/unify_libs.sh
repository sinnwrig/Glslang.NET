#!/bin/bash
#
# Creates one static library from several.
#
# Usage: copy all your libs to a single directory and call this script.
#

if [[ $# < 4 ]]; then
  echo "Usage: ./unify_libs.sh output-file library-path library-file(s)"
  exit 2
fi


# Inputs
LIBNAME=$1
LIBSDIR=$2
LIBS="${@:3}"

rm -rf ${OBJDIR}
# Tmp dir
OBJDIR=/tmp/unify-static-libs
mkdir -p ${OBJDIR}
# Extract .o
echo "Extracting objects to ${OBJDIR}..."

for i in ${LIBS}
do
    LIBPATH="$LIBSDIR/$i.a"
    OUTDIR="$OBJDIR/$i"

    mkdir -p ${OUTDIR}

    echo "$LIBPATH"

    ar --output $OUTDIR -x $LIBPATH
done


# Link objects into a single lib
echo "Creating $LIBNAME from objects..."

FULLLIBPATH=""

for i in ${LIBS}
do
    FULLLIBPATH+=" $OBJDIR/$i/*.o"
done

#echo $FULLLIBPATH

ar -crs $LIBNAME $FULLLIBPATH
## Clean up
rm -rf ${OBJDIR}
echo "Done."

